using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public interface IDynamicRepository
{
    Task<bool> FieldExistsAsync(string tableName, string fieldName);
    Task AddFieldAsync(string tableName, string fieldName, string fieldType);
    Task<int> CreateAndAddObjectAsync(string tableName, Dictionary<string, object> data);
    Task<List<Dictionary<string, object>>> GetAllCustomersAsync();
    Task DeleteCustomerAsync(int customerId);
}
