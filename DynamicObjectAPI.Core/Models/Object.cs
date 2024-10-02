using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Core.Models
{
    public class Object
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Field> Fields { get; set; }
    }
}
