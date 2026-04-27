using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public record ProductRequest(
        int BrandId,
        int CategoryId,
        string Name,
        string Description,
        decimal Price,
        int QuantityAvailable
        );
    
}
