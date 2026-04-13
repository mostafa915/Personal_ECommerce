using ECommerce.Application.DTOs.Brands;
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
    public class BrandRepo(ApplicationDbContext context) : IBrandRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Brand brand, CancellationToken cancellationToken) =>
            await _context.AddAsync(brand, cancellationToken);

        public async Task<bool> AnyHasName(int id, string name, CancellationToken cancellationToken) =>
            await _context.Brands.AnyAsync(x => x.Name == name && x.Id != id, cancellationToken);

        public async Task<bool> AnyHasName(string name, CancellationToken cancellationToken) =>
            await _context.Brands.AnyAsync(x => x.Name == name, cancellationToken);

        public async Task<IEnumerable<BrandResponse>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Brands.AsNoTracking().ProjectToType<BrandResponse>().ToListAsync(cancellationToken);

        public async Task<IEnumerable<BrandResponse>> GetAllAvailableAsync(CancellationToken cancellationToken) =>
            await _context.Brands.Where(x => x.IsAvailable).AsNoTracking().ProjectToType<BrandResponse>().ToListAsync(cancellationToken);

        public async Task<Brand?> GetAvailableByIdAsync(int id, CancellationToken cancellationToken) =>
            await _context.Brands.SingleOrDefaultAsync(x => x.Id == id && x.IsAvailable, cancellationToken);

        public async Task<Brand?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await _context.Brands.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);

        public void Update(Brand brand) =>
            _context.Update(brand);
    }
}
