using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokeOn { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpireOn;

        public bool IsActive => RevokeOn is null && !IsExpired;
    }
}
