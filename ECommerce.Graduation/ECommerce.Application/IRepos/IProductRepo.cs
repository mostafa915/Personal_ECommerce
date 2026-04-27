using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IRepos
{
    public interface IProductRepo
    {
        Task AddAsync(Product product, CancellationToken cancellationToken);
        void Update(Product product);
        Task<IEnumerable<ProductResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<ProductResponse>> GetAllAvailableAsync(CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Product?> GetAvailableByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsNameWithCategoryAndBrandExist(string name, int categoryId, int brandId, CancellationToken cancellationToken);
        Task<bool> AnyAsyncHasIdAndAvailable(int id, CancellationToken cancellationToken);
        Task<bool> IsNameWithCategoryAndBrandExist(int id, string name, int categoryId, int brandId, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
