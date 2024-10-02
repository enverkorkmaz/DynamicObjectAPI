using Newtonsoft.Json.Linq;

namespace DynamicObjectAPI.Api.DTOs
{
    public class CreateObjectRequest
    {
        public string ObjectType { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

}
