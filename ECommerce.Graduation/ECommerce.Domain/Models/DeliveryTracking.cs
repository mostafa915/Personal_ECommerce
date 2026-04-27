using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public enum DeliveryTracking
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
