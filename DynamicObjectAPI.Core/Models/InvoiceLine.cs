using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Core.Models
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; } 

        public Invoice Invoice { get; set; } 
    }
}
