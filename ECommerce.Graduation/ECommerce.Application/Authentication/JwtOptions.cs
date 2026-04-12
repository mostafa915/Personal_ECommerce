using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Authentication
{
    public class JwtOptions
    {
        public static string SectionName = "Jwt";

        [Required]
        public string Issuer { get; init; } = string.Empty;
        [Required]
        public string Audience { get; init; } = string.Empty;
        [Required]
        public string Key { get; init; } = string.Empty;
        [Range(1, int.MaxValue)]
        public int ExpiresIn { get; set; }
    }
}
