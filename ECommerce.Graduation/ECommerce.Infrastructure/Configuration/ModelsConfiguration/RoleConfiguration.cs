using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Configuration.ModelsConfiguration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData([
                new ApplicationRole 
                {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
                },

                new ApplicationRole 
                {
                    Id = DefaultRoles.CustomerRoleId,
                    Name = DefaultRoles.Customer,
                    NormalizedName= DefaultRoles.Customer.ToUpper(),
                    ConcurrencyStamp= DefaultRoles.CustomerRoleConcurrencyStamp,
                    IsDefault = true,
                }

                ]);
        }
    }
}
