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
    internal class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(p => p.FirstName)
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .HasMaxLength(100);

            builder
                .OwnsMany(u => u.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");

            builder.HasData(new ApplicationUser
            {
                Id = DefaultUser.AdminId,
                Email = DefaultUser.AdminEmail,
                NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
                FirstName = DefaultUser.AdminFirstName,
                LastName = DefaultUser.AdminLastName,
                UserName = DefaultUser.AdminEmail,
                NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = DefaultUser.AdminPasswordHash,
                SecurityStamp = DefaultUser.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp, 
            }
            );

        }
    }
}
