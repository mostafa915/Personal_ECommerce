using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class CartsError
    {
        public static readonly Error NotFound = new("Carts.NotFound", "Cart Is Not Found", StatusCodes.Status404NotFound);
        public static readonly Error ItemNotFound = new("Carts.ItemNotFound", "Item is not found", StatusCodes.Status404NotFound);
        public static readonly Error NoHasItems = new("Carts.NoHasItems", "Cart hasn't any item to apply your order", StatusCodes.Status404NotFound);
    }
}
