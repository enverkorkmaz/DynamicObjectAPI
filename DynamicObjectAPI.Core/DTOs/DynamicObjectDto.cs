using DynamicObjectAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Core.DTOs
{
    public class DynamicObjectDto
    {
        public string ObjectType { get; set; }
        public Dictionary<string, object> Fields { get; set; }
    }
}
