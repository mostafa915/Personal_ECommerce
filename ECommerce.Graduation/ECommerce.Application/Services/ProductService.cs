using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Errors;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService(IProductRepo productRepo, IBrandRepo brandRepo, ICategoryRepo categoryRepo) : IProductService
    {
        private readonly IProductRepo _productRepo = productRepo;
        private readonly IBrandRepo _brandRepo = brandRepo;
        private readonly ICategoryRepo _categoryRepo = categoryRepo;

        
        public async Task<Result<IEnumerable<ProductResponse>>> GetAllAsync(string? search ,int? categoryId, int? brandId, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync(cancellationToken);

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.Name.ToLower().Contains(search.ToLower()));    
            }

            if (categoryId.HasValue)
            {
                var IsCategoryExists = await _categoryRepo.AnyAsyncHasId((int)categoryId, cancellationToken);
                if (!IsCategoryExists)
                    Result.Faliuar<IEnumerable<ProductResponse>>(CategoriesError.NotFound);

                products = products.Where(x => x.CategoryId == categoryId);
            }

            if(brandId.HasValue)
            {
                var IsBrandExists = await _brandRepo.AnyAsyncHasId((int) brandId, cancellationToken);
                if(!IsBrandExists)
                  return Result.Faliuar<IEnumerable<ProductResponse>>(BrandsError.NotFound);
                
                products = products.Where(x => x.BrandId == brandId);
            }

            return Result.Success(products);
        }
        
        public async Task<Result<IEnumerable<ProductResponse>>> GetAllAvailableAsync(string? search, int? categoryId, int? brandId, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAvailableAsync(cancellationToken);

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }

            if (categoryId.HasValue)
            {
                var IsCategoryAvailableAndExists = await _categoryRepo.AnyAsyncHasIdAndAvailable((int)categoryId,cancellationToken);
                if (!IsCategoryAvailableAndExists)
                    return Result.Faliuar<IEnumerable<ProductResponse>>(CategoriesError.NotFound);

                products = products.Where(x => x.CategoryId == categoryId);
            }

            if(brandId.HasValue)
            {
                var IsBrandAvailableAndExists = await _brandRepo.AnyAsyncHasIdAndAvailable((int)brandId, cancellationToken);
                if(!IsBrandAvailableAndExists)
                    return Result.Faliuar<IEnumerable<ProductResponse>>(BrandsError.NotFound);

                products = products.Where(x => x.BrandId == brandId);
            }

            return Result.Success(products);
        }

        public async Task<Result<ProductResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _productRepo.GetByIdAsync(id, cancellationToken) is not { } product)
                return Result.Faliuar<ProductResponse>(ProductsError.NotFound);

            return Result.Success(product.Adapt<ProductResponse>());
        }


        public async Task<Result<ProductResponse>> GetAvailableAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _productRepo.GetAvailableByIdAsync(id, cancellationToken) is not { } product)
                return Result.Faliuar<ProductResponse>(ProductsError.NotFound);

            return Result.Success(product.Adapt<ProductResponse>());

            
        }
        
        public async Task<Result<ProductResponse>> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default)
        {
            if(await _brandRepo.GetAvailableByIdAsync(request.BrandId, cancellationToken) is not { } brand)
                return Result.Faliuar<ProductResponse>(BrandsError.NotFound);

            if (await _categoryRepo.GetAvaliableByIdAsync(request.CategoryId, cancellationToken) is not { } category)
                return Result.Faliuar<ProductResponse>(CategoriesError.NotFound);

            var IsProductAlreadyExist = await _productRepo.IsNameWithCategoryAndBrandExist(request.Name, request.CategoryId, request.BrandId, cancellationToken);
            if (IsProductAlreadyExist)
                return Result.Faliuar<ProductResponse>(ProductsError.DuplicateProductName);

            var product = request.Adapt<Product>();
            await _productRepo.AddAsync(product, cancellationToken);
            await _productRepo.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Adapt<ProductResponse>());
        }

        public async Task<Result> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            if(await _productRepo.GetByIdAsync(id, cancellationToken) is not { } product)
                return Result.Faliuar(ProductsError.NotFound);

            var IsDupilcatedName = await _productRepo.IsNameWithCategoryAndBrandExist(id, request.Name, product.CategoryId, product.BrandId, cancellationToken);
            if(IsDupilcatedName)
                return Result.Faliuar(ProductsError.DuplicateProductName);

             product = request.Adapt(product);

            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateQuantityAsync(int id, UpdateProductQuatityRequest request, CancellationToken cancellationToken = default)
        {
            if (await _productRepo.GetByIdAsync(id, cancellationToken) is not { } product)
                return Result.Faliuar(ProductsError.NotFound);

            product.QuantityAvailable = request.QuantityAvailable;
            await _productRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }


        public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _productRepo.GetByIdAsync(id, cancellationToken) is not { } product)
                return Result.Faliuar(ProductsError.NotFound);

            product.IsAvailable = !product.IsAvailable;
            await _productRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        
    }
}
