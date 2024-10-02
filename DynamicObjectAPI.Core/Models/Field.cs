using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Core.Models
{
    public class Field
    {
        public int Id { get; set; }
        public int ObjectId { get; set; } 
        public string FieldName { get; set; } 
        public string FieldType { get; set; } 
        public Object Object { get; set; }
    }
}
