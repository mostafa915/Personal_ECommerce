using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IBrandService
    {
        Task<Result<IEnumerable<BrandResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<BrandResponse>>> GetAllAvailableAsync(CancellationToken cancellationToken = default);
        Task<Result<BrandResponse>> GetAsync(int id ,CancellationToken cancellationToken = default);
        Task<Result<BrandResponse>> GetAvailableAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<BrandResponse>> CreatAsync(BrandRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default);


    }
}
