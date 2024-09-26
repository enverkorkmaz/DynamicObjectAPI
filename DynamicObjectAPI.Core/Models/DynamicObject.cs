using DynamicObjectAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Core.Models
{
    public class DynamicObject : BaseEntity
    {
        public string ObjectType { get; set; }
        public Dictionary<string, object> Fields { get; set; }
    }
}
