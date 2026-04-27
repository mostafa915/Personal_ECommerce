using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public class Order : EditableEntity
    {
        public int Id { get; set; } 
        public string Address { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DeliveryTracking OrderStatus { get; set; } = DeliveryTracking.Pending;
        public ICollection<OrderItem> OrderItems { get; set; } = [];

    }
}
