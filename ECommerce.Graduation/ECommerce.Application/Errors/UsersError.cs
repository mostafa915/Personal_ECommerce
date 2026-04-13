using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class UsersError
    {

        public static readonly Error InValidCrendtial = new("User.InValidCrendtial", "Invalid Email/Password", StatusCodes.Status401Unauthorized);
        public static readonly Error NotFound = new("User.NotFound", "User With This Id Is Not Found", StatusCodes.Status404NotFound);
        public static readonly Error EmailIsExistAlready = new("User.EmailIsExistAlready", "This Email Is Already Exist, Please Enter anthor email", StatusCodes.Status409Conflict);

    }
}
