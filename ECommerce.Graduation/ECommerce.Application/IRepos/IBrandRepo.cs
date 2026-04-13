using ECommerce.Application.DTOs.Brands;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IRepos
{
    public interface IBrandRepo
    {
        Task<IEnumerable<BrandResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<BrandResponse>> GetAllAvailableAsync(CancellationToken cancellationToken);
        Task<Brand?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Brand?> GetAvailableByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync (Brand brand, CancellationToken cancellationToken);
        void Update(Brand brand);
        Task<bool> AnyHasName(int id, string name, CancellationToken cancellationToken);
        Task<bool> AnyHasName(string name, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
