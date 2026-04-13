using ECommerce.Application.IRepos;
using ECommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repos
{
    public class AuthRepo(ApplicationDbContext context) : IAuthRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> AnyEmailAsync(string email, CancellationToken cancellationToken) =>
            await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        
    }
}
