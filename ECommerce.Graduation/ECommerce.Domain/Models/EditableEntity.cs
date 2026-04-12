using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public class EditableEntity
    {
        public string CreatedById { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? UpdateById { get; set; }
        public DateTime? UpdateOn { get; set; }

        public ApplicationUser CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
