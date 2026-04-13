using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<CategoryResponse>>> GetAllAvailableAsync(CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> GetAvaliableAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
    }
}
