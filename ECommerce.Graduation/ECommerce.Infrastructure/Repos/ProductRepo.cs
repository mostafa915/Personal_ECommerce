using ECommerce.Application.DTOs.Products;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repos
{
    public class ProductRepo(ApplicationDbContext context) : IProductRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.AddAsync(product, cancellationToken);
        }

        public async Task<bool> AnyAsyncHasIdAndAvailable(int id, CancellationToken cancellationToken) =>
            await _context.Products.AnyAsync(x => x.Id == id && x.IsAvailable, cancellationToken);

        public async Task<IEnumerable<ProductResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Select(x => new ProductResponse
                (
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.QuantityAvailable,
                    x.BrandId,
                    x.Brand.Name,
                    x.CategoryId,
                    x.Category.Name
                )
                )
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAvailableAsync(CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(x => x.IsAvailable)
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Select(x => new ProductResponse
                (
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.QuantityAvailable,
                    x.BrandId,
                    x.Brand.Name,
                    x.CategoryId,
                    x.Category.Name
                )
                )
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetAvailableByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.Include(x => x.Brand).Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id && x.IsAvailable, cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.Include(x => x.Brand).Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> IsNameWithCategoryAndBrandExist(string name, int categoryId, int brandId, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.Name ==  name && x.CategoryId == categoryId && x.BrandId == brandId, cancellationToken);
        }

        public async Task<bool> IsNameWithCategoryAndBrandExist(int id, string name, int categoryId, int brandId, CancellationToken cancellationToken)
        {
           return await _context.Products.AnyAsync(x => x.Id != id && x.Name == name && x.CategoryId == categoryId && x.BrandId == brandId, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public void Update(Product product)
        {
            _context.Update(product);
        }
    }
}
