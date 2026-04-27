using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class ProductsError
    {
        public static readonly Error DuplicateProductName = new("Products.DuplicateName"
         , "This Product name already exists, please choose a different one."
         , StatusCodes.Status409Conflict);

        public static readonly Error NotFound = new("Produts.NotFound", "Product Is Not Found", StatusCodes.Status404NotFound);
        public static readonly Error QuantityIsNotEnough = new("Products.QuantityIsNotEnough", "Quantity is not enough", StatusCodes.Status409Conflict);

    }
}
