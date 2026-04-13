using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Categories;
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
    public class CategoryService(ICategoryRepo categoryRepo) : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo = categoryRepo;

        public async Task<Result<IEnumerable<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepo.GetAllAsync(cancellationToken);
            return Result.Success(categories);
        }

        public async Task<Result<IEnumerable<CategoryResponse>>> GetAllAvailableAsync(CancellationToken cancellationToken = default)
        {
            var avaliableCategories = await _categoryRepo.GetAllAvailableAsync(cancellationToken);
            return Result.Success(avaliableCategories);
        }

        public async Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _categoryRepo.GetByIdAsync(id, cancellationToken) is not { } category)
                return Result.Faliuar<CategoryResponse>(CategoriesError.NotFound);

            return Result.Success(category.Adapt<CategoryResponse>());
        }

        public async Task<Result<CategoryResponse>> GetAvaliableAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _categoryRepo.GetAvaliableByIdAsync(id, cancellationToken) is not { } category)
                return Result.Faliuar<CategoryResponse>(CategoriesError.NotFound);

            return Result.Success(category.Adapt<CategoryResponse>());
        }


        public async Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default)
        {
            var IsCatgoryDupilcatd = await _categoryRepo.AnyHasName(request.Name, cancellationToken);
            if (IsCatgoryDupilcatd)
                return Result.Faliuar<CategoryResponse>(CategoriesError.DuplicateCategoryName);

            var category = request.Adapt<Category>();
            await _categoryRepo.AddAsync(category, cancellationToken);
            await _categoryRepo.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Adapt<CategoryResponse>());
        }

        public async Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepo.GetByIdAsync(id, cancellationToken);
            if(category is null)
                return Result.Faliuar(CategoriesError.NotFound);

            var IsDupilcatedName = await _categoryRepo.AnyHasName(id, request.Name, cancellationToken);
            if (IsDupilcatedName)
                return Result.Faliuar(CategoriesError.DuplicateCategoryName);

            category = request.Adapt(category);
             
            _categoryRepo.Update(category);
            await _categoryRepo.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> ToggleAsync(int id, CancellationToken cancellationToken = default)
        {
            if (await _categoryRepo.GetByIdAsync(id, cancellationToken) is not { } category)
                return Result.Faliuar(CategoriesError.NotFound);

            category.IsAvailable = !category.IsAvailable;
            await _categoryRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
