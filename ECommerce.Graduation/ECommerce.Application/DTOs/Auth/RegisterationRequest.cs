using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Auth
{
    public record RegisterationRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName
        );
}
