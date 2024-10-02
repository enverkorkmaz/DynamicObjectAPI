using DynamicObjectAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Object = DynamicObjectAPI.Core.Models.Object;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Object> Objects { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<InvoiceLine> InvoiceLine { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Field>()
            .HasOne(f => f.Object)
            .WithMany(o => o.Fields)
            .HasForeignKey(f => f.ObjectId);

        
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CustomerId);

        
        modelBuilder.Entity<InvoiceLine>()
            .HasOne(il => il.Invoice)
            .WithMany(i => i.InvoiceLines)
            .HasForeignKey(il => il.InvoiceId);

        
        modelBuilder.Entity<Object>().HasData(
            new Object { Id = 1, Name = "Customer" },
            new Object { Id = 2, Name = "Invoice" },
            new Object { Id = 3, Name = "InvoiceLine" }
        );

        
        modelBuilder.Entity<Field>().HasData(
            new Field { Id = 1, ObjectId = 1, FieldName = "Name", FieldType = "string" },
            new Field { Id = 2, ObjectId = 2, FieldName = "InvoiceNumber", FieldType = "string" },
            new Field { Id = 3, ObjectId = 3, FieldName = "ItemName", FieldType = "string" },
            new Field { Id = 4, ObjectId = 3, FieldName = "Price", FieldType = "decimal" }
        );
    }
}
