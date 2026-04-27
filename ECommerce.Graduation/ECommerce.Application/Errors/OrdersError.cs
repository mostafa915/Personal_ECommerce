using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class OrdersError
    {
        public static readonly Error InvaildStatusTranstion = new("OrderError.InvaildStatusTranstion", "Invaild Status Transtion", StatusCodes.Status400BadRequest);
        public static readonly Error StatusNotExists = new("OrderError.StatusNotExists", "Status not exists", StatusCodes.Status400BadRequest);
        public static readonly Error NotFound = new("OrderError.NotFound", "Order Is Not Found", StatusCodes.Status404NotFound);
        public static readonly Error Failed = new("OrdersError.Failed", "The Opreation Failed", StatusCodes.Status500InternalServerError);
    }
}
