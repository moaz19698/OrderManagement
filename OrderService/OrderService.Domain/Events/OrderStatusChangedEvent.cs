using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Events
{
    public class OrderStatusChangedEvent
    {
        public Guid OrderId { get; set; }
        public string NewStatus { get; set; }
    }

}
