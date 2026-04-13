using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repos
{
    public class CategoryRepo(ApplicationDbContext context) : ICategoryRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Category category, CancellationToken cancellationToken) =>
            await _context.AddAsync(category, cancellationToken);

        
        public async Task<bool> AnyHasName(string name, CancellationToken cancellationToken) =>
            await _context.Categories.AnyAsync(x => x.Name == name, cancellationToken);

        public async Task<bool> AnyHasNameAndAvailable(string name, CancellationToken cancellationToken) =>
            await _context.Categories.AnyAsync(x => x.Name == name && x.IsAvailable, cancellationToken);

        public async Task<bool> AnyHasName(int id, string name, CancellationToken cancellationToken) =>
            await _context.Categories.AnyAsync(x => x.Name == name && x.Id != id, cancellationToken);


        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Categories.AsNoTracking().ProjectToType<CategoryResponse>().ToListAsync(cancellationToken);


        public async Task<IEnumerable<CategoryResponse>> GetAllAvailableAsync(CancellationToken cancellationToken) =>
            await _context.Categories.Where(x => x.IsAvailable).AsNoTracking().ProjectToType<CategoryResponse>().ToListAsync();


        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await _context.Categories.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => 
            await _context.SaveChangesAsync(cancellationToken);

        public void Update(Category category) =>
            _context.Update(category);

        public async Task<Category?> GetAvaliableByIdAsync(int id, CancellationToken cancellationToken) =>
            await _context.Categories.SingleOrDefaultAsync(x => x.Id == id && x.IsAvailable, cancellationToken);
    }
}
