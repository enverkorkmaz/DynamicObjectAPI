using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public interface IDynamicService
{
    Task<int> CreateObjectAsync(string objectType, Dictionary<string, object> data);
    Task<List<Dictionary<string, object>>> GetAllCustomersAsync();
    Task DeleteCustomerAsync(int customerId);

}
