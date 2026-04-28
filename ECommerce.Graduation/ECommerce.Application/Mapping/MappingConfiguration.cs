using ECommerce.Application.DTOs.Products;
using ECommerce.Application.DTOs.Reviews;
using ECommerce.Domain.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, ProductResponse>()
                .Map(x => x.BrandName, y => y.Brand.Name)
                .Map(x => x.CategoryName, y => y.Category.Name);

            config.NewConfig<Review, UserReviewResponse>()
                .Map(x => x.UserName, y => y.User.FirstName + " " + y.User.LastName);
        }
    }
}
