using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class BrandsError
    {
        public static readonly Error DuplicateBrandName = new("Brands.DuplicateName"
          , "This Brand name already exists, please choose a different one."
          , StatusCodes.Status409Conflict);

        public static readonly Error NotFound = new("Brands.NotFound", "Brand With This Id Is Not Found", StatusCodes.Status404NotFound);

    }
}
