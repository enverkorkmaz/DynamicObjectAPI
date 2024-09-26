using System.Text.Json;

namespace DynamicObjectAPI.Service.Helpers
{
    public static class DynamicObjectHelper
    {
        
        public static void ConvertFields(Dictionary<string, object> fields)
        {
            foreach (var field in fields.Keys.ToList())
            {
                if (fields[field] is JsonElement jsonElement)
                {
                    fields[field] = ConvertJsonElement(jsonElement);
                }
            }
        }

        
        private static object ConvertJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out var l) ? l : (object)element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Object => JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText()),
                JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElement).ToList(),
                _ => null
            };
        }
    }
}
