using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; } // New, Approved, Dispatched
        public DateTime CreatedAt { get; set; }
    }

}
