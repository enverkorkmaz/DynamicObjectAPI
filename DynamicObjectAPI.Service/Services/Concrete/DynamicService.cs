using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class DynamicService : IDynamicService
{
    private readonly IDynamicRepository _dynamicRepository;

    public DynamicService(IDynamicRepository dynamicRepository)
    {
        _dynamicRepository = dynamicRepository;
    }

    
    public async Task<int> CreateObjectAsync(string objectType, Dictionary<string, object> data)
    {
        if (string.IsNullOrEmpty(objectType))
        {
            throw new ArgumentNullException(nameof(objectType), "Object type is required.");
        }

        
        return await _dynamicRepository.CreateAndAddObjectAsync(objectType,  data);
    }

    public async Task<List<Dictionary<string, object>>> GetAllCustomersAsync()
    {
        return await _dynamicRepository.GetAllCustomersAsync();
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        await _dynamicRepository.DeleteCustomerAsync(customerId);
    }
   
}
