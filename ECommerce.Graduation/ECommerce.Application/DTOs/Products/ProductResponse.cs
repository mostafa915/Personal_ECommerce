using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public record ProductResponse(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int QuantityAvailable,
        int BrandId,
        string BrandName,
        int CategoryId,
        string CategoryName
        );
}
