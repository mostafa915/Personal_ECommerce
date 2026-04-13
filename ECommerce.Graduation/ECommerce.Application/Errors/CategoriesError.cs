using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class CategoriesError
    {
        public static readonly Error DuplicateCategoryName = new("Categories.DuplicateName"
         , "This category name already exists, please choose a different one."
         , StatusCodes.Status409Conflict);

        public static readonly Error NotFound = new("Categories.NotFound", "category Is Not Found", StatusCodes.Status404NotFound);

    }
}
