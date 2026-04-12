using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Exteions_Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Configuration
{

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var CascadeFKs = builder.Model
               .GetEntityTypes()
               .SelectMany(f => f.GetForeignKeys())
               .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in CascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<EditableEntity>();
            foreach (var entry in entries)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedById).CurrentValue = currentUserId!;
                }

                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(p => p.UpdateById).CurrentValue = currentUserId!;
                    entry.Property(p => p.UpdateOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
