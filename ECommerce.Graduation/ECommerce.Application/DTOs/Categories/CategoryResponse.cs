using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Categories
{
    public record CategoryResponse(
        int Id,
        string Name,
        string Description,
        bool IsAvailable
        );
    
}
