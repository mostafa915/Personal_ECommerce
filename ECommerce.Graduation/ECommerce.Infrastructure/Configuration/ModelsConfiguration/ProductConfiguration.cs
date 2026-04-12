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
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasIndex(x => new {x.Name, x.BrandId, x.CategoryId})
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(250);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);
          
            builder.HasCheckConstraint("Product_Price", "[Price] > 0");
        }
    }
}
