using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Brands;
using ECommerce.Application.Errors;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class BrandService(IBrandRepo brandRepo) : IBrandService
    {
        private readonly IBrandRepo _brandRepo = brandRepo;

        public async Task<Result<IEnumerable<BrandResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var brands = await _brandRepo.GetAllAsync(cancellationToken);
            return Result.Success(brands);
        }

        public async Task<Result<IEnumerable<BrandResponse>>> GetAllAvailableAsync(CancellationToken cancellationToken = default)
        {
            var availableBrands = await _brandRepo.GetAllAvailableAsync(cancellationToken);
            return Result.Success(availableBrands);
        }

        public async Task<Result<BrandResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _brandRepo.GetByIdAsync(id, cancellationToken) is not { } brand)
                return Result.Faliuar<BrandResponse>(BrandsError.NotFound);

            return Result.Success(brand.Adapt<BrandResponse>());
        }

        public async Task<Result<BrandResponse>> GetAvailableAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _brandRepo.GetAvailableByIdAsync(id, cancellationToken) is not { } brand)
                return Result.Faliuar<BrandResponse>(BrandsError.NotFound);

            return Result.Success(brand.Adapt<BrandResponse>());
        }

        public async Task<Result<BrandResponse>> CreatAsync(BrandRequest request, CancellationToken cancellationToken = default)
        {
            var IsNameDupilcated = await _brandRepo.AnyHasName(request.Name, cancellationToken);
            if (IsNameDupilcated)
                return Result.Faliuar<BrandResponse>(BrandsError.DuplicateBrandName);

            var brand = request.Adapt<Brand>();
            await _brandRepo.AddAsync(brand, cancellationToken);
            await _brandRepo.SaveChangesAsync(cancellationToken);

            return Result.Success(brand.Adapt<BrandResponse>());
        }

        public async Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken = default)
        {
            if (await _brandRepo.GetByIdAsync(id, cancellationToken) is not { } brand)
                return Result.Faliuar(BrandsError.NotFound);

            var IsNameDupilcated = await _brandRepo.AnyHasName(id, request.Name, cancellationToken);
            if (IsNameDupilcated)
                return Result.Faliuar(BrandsError.DuplicateBrandName);

            brand = request.Adapt(brand);
            
            _brandRepo.Update(brand);
            await _brandRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();
            
        }

        public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _brandRepo.GetByIdAsync(id, cancellationToken) is not { } brand)
                return Result.Faliuar(BrandsError.NotFound);

            brand.IsAvailable = !brand.IsAvailable;
            await _brandRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
