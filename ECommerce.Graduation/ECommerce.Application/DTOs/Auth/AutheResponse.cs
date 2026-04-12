using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Auth
{
    public record AutheResponse(
        string Id,
        string? Email,
        string FirstName,
        string LastName,
        string Token,
        int ExpireIn,
        string RefreshToken,
        DateTime RefreshTokenExpiretion
        );
}
