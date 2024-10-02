using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DynamicObjectAPI.Core.Models;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;

public class DynamicRepository : IDynamicRepository
{
    private readonly AppDbContext _context;

    public DynamicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> FieldExistsAsync(string tableName, string fieldName)
    {
        var objectEntity = await _context.Objects.FirstOrDefaultAsync(o => o.Name == tableName);
        if (objectEntity == null)
        {
            return false;
        }

        return await _context.Fields.AnyAsync(f => f.ObjectId == objectEntity.Id && f.FieldName == fieldName);
    }

    public async Task AddFieldAsync(string tableName, string fieldName, string fieldType)
    {
        var objectEntity = await _context.Objects.FirstOrDefaultAsync(o => o.Name == tableName);
        if (objectEntity == null)
        {
            throw new Exception($"Object '{tableName}' not found in the Object table.");
        }

        var newField = new Field
        {
            ObjectId = objectEntity.Id,
            FieldName = fieldName,
            FieldType = fieldType
        };
        await _context.Fields.AddAsync(newField);
        await _context.SaveChangesAsync();

        var alterTableQuery = $"ALTER TABLE {tableName} ADD {fieldName} {fieldType}";
        await _context.Database.ExecuteSqlRawAsync(alterTableQuery);
    }

    public async Task<int> CreateAndAddObjectAsync(string tableName, Dictionary<string, object> data)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            var filteredData = data
                .Where(d => d.Key != "Invoices" && d.Key != "InvoiceLines") 
                .ToDictionary(d => d.Key, d => d.Value);

            
            var columns = string.Join(",", filteredData.Keys);
            var values = string.Join(",", filteredData.Values.Select(v => $"'{v}'"));
            var insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values}); SELECT SCOPE_IDENTITY();";

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = insertQuery;
            var customerId = Convert.ToInt32(await command.ExecuteScalarAsync());

            
            if (data.ContainsKey("Invoices"))
            {
                var invoicesObj = data["Invoices"];

                if (invoicesObj is JsonElement invoicesElement && invoicesElement.ValueKind == JsonValueKind.Array)
                {
                    var invoices = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(invoicesElement.GetRawText());

                    foreach (var invoiceData in invoices)
                    {
                        var invoiceTable = "Invoice";

                        
                        invoiceData["CustomerId"] = customerId; 
                        var invoiceColumns = string.Join(",", invoiceData.Where(d => d.Key != "InvoiceLines").Select(d => d.Key));
                        var invoiceValues = string.Join(",", invoiceData.Where(d => d.Key != "InvoiceLines").Select(v => $"'{v.Value}'"));
                        insertQuery = $"INSERT INTO {invoiceTable} ({invoiceColumns}) VALUES ({invoiceValues}); SELECT SCOPE_IDENTITY();";

                        command.CommandText = insertQuery;
                        var invoiceId = Convert.ToInt32(await command.ExecuteScalarAsync());

                        
                        if (invoiceData.ContainsKey("InvoiceLines") && invoiceData["InvoiceLines"] is JsonElement invoiceLinesElement && invoiceLinesElement.ValueKind == JsonValueKind.Array)
                        {
                            var invoiceLines = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(invoiceLinesElement.GetRawText());

                            foreach (var lineData in invoiceLines)
                            {
                                var invoiceLineTable = "InvoiceLine";

                                
                                lineData["InvoiceId"] = invoiceId; 
                                var lineColumns = string.Join(",", lineData.Select(d => d.Key));
                                var lineValues = string.Join(",", lineData.Select(v => $"'{v.Value}'"));
                                insertQuery = $"INSERT INTO {invoiceLineTable} ({lineColumns}) VALUES ({lineValues});";

                                command.CommandText = insertQuery;
                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception($"Invoices verisi beklenen formatta değil veya boş. Gelen veri tipi: {invoicesObj?.GetType()}");
                }
            }

            
            await transaction.CommitAsync();
            return customerId;
        }
        catch (Exception)
        {
            
            await transaction.RollbackAsync();
            throw; 
        }
    }

    public async Task<List<Dictionary<string, object>>> GetAllCustomersAsync()
    {
        var results = new List<Dictionary<string, object>>();

        var customers = await _context.Customer.ToListAsync();

        await using var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        foreach (var customer in customers)
        {
            var record = new Dictionary<string, object>
                {
                    { "Id", customer.Id },
                    { "Name", customer.Name }
                };

            
            var fields = await _context.Fields
                .Where(f => f.ObjectId == customer.Id && f.Object.Name == "Customer")
                .ToListAsync();



            foreach (var field in fields)
            {
                
                var selectQuery = $"SELECT [{field.FieldName}] FROM Customer WHERE Id = {customer.Id}";



                using var command = connection.CreateCommand();
                command.CommandText = selectQuery;

                var fieldValue = await command.ExecuteScalarAsync();
                record[field.FieldName] = fieldValue;
            }

            
            var invoices = await _context.Invoice
                .Where(i => i.CustomerId == customer.Id)
                .ToListAsync();

            var invoiceList = new List<Dictionary<string, object>>();

            foreach (var invoice in invoices)
            {
                var invoiceRecord = new Dictionary<string, object>
                    {
                        { "Id", invoice.Id },
                        { "InvoiceNumber", invoice.InvoiceNumber },
                        { "CustomerId", invoice.CustomerId }
                    };

               
                var invoiceLines = await _context.InvoiceLine
                    .Where(il => il.InvoiceId == invoice.Id)
                    .ToListAsync();

                var invoiceLineList = new List<Dictionary<string, object>>();

                foreach (var line in invoiceLines)
                {
                    var lineRecord = new Dictionary<string, object>
                        {
                            { "Id", line.Id },
                            { "ItemName", line.ItemName },
                            { "Price", line.Price },
                            { "InvoiceId", line.InvoiceId }
                        };

                    invoiceLineList.Add(lineRecord);
                }

                invoiceRecord["InvoiceLines"] = invoiceLineList;
                invoiceList.Add(invoiceRecord);
            }

            record["Invoices"] = invoiceList;
            results.Add(record);
            connection.Close();
        }

        return results;
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            
            var customer = await _context.Customer
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                
                await transaction.CommitAsync();
                return;
            }

            
            var invoiceLines = await _context.InvoiceLine
                .Where(il => _context.Invoice.Any(i => i.Id == il.InvoiceId && i.CustomerId == customerId))
                .ToListAsync();

            if (invoiceLines.Any())
            {
                _context.InvoiceLine.RemoveRange(invoiceLines);
            }

           
            var invoices = await _context.Invoice
                .Where(i => i.CustomerId == customerId)
                .ToListAsync();

            if (invoices.Any())
            {
                _context.Invoice.RemoveRange(invoices);
            }

            
            _context.Customer.Remove(customer);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
   
}


