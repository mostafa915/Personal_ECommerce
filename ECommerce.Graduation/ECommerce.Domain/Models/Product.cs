using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public class Product : EditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Brand Brand { get; set; } = default!;
        public Category Category { get; set; } = default!;
        public ICollection<CartItem> cartItems { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}
