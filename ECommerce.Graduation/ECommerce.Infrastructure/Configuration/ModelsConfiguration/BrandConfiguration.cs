using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Configuration.ModelsConfiguration
{
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(250);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);
        }
    }
}
