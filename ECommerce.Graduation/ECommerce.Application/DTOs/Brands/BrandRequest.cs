using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Brands
{
    public record BrandRequest(
        string Name,
        string Description
        );
}
