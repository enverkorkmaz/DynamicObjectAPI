using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Core.Models
{
    public class DynamicField
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public DynamicField(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Key cannot be null or empty.");
            Key = key;
            Value = value;
        }
    }
}
