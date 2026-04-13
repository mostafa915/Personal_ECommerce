using ECommerce.Application.DTOs.Categories;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IRepos
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<CategoryResponse>> GetAllAvailableAsync(CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Category?> GetAvaliableByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> AnyHasName(string name, CancellationToken cancellationToken);
        Task<bool> AnyHasName(int id ,string name, CancellationToken cancellationToken);
        Task<bool> AnyHasNameAndAvailable(string name, CancellationToken cancellationToken);
        Task AddAsync(Category category, CancellationToken cancellationToken);
        void Update(Category category);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
