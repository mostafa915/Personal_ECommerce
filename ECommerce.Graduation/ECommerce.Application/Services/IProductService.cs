using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductResponse>>> GetAllAsync(string? search, int? categoryId, int? brandId, CancellationToken cancellationToken);
        Task<Result<ProductResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<ProductResponse>>> GetAllAvailableAsync(string? search, int? categoryId, int? brandId, CancellationToken cancellationToken);
        Task<Result<ProductResponse>> GetAvailableAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<ProductResponse>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateQuantityAsync(int id, UpdateProductQuatityRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);
    }
}
