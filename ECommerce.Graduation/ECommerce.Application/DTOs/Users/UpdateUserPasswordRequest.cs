using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Users
{
    public record UpdateUserPasswordRequest(
        string OldPass,
        string NewPass
        );
}
