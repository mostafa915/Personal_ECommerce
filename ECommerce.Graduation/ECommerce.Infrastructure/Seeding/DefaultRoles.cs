using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Seeding
{
    public static class DefaultRoles
    {
        public const string Admin = nameof(Admin);
        public const string AdminRoleId = "635C2650-297D-4B2D-8A9A-BD8534964C28";
        public const string AdminRoleConcurrencyStamp = "804ACDF6-7E33-4DB1-925B-C34F8E6ACE89";

        public const string Customer = nameof(Customer);
        public const string CustomerRoleId = "CEC63D21-2FBB-481C-8950-527E87A31A69";
        public const string CustomerRoleConcurrencyStamp = "C7156965-B0B4-48ED-954C-6F14264BFEBD";
    }
}
