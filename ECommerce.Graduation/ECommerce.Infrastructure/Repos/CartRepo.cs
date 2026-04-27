using ECommerce.Application.DTOs.Carts;
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
    public class CartRepo(ApplicationDbContext context) : ICartRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Cart cart, CancellationToken cancellationToken) =>
            await _context.AddAsync(cart, cancellationToken);

        public async Task AddAsync(CartItem itme, CancellationToken cancellationToken) =>
            await _context.AddAsync(itme, cancellationToken);

        public async Task<Cart?> GetCartAsync(string userId, CancellationToken cancellationToken) =>
            await _context.Carts.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        public async Task<CartItem?> GetItemByProductIdAsync(int cartId, int productId, CancellationToken cancellationToken) =>
            await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId, cancellationToken);

        public async Task<IEnumerable<CartItem>> GetItemsAsync(int cartId, CancellationToken cancellationToken) =>
            await _context.CartItems
            .Where(x => x.CartId == cartId)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);

        public void Remove(CartItem cartItem) =>
            _context.Remove(cartItem);


        public void ClearItems(IEnumerable<CartItem> items) =>
            _context.RemoveRange(items);
        public async Task<int> SavaChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);

        public void Update(Cart cart)
        {
            _context.Update(cart);
        }

        public void Update(CartItem cartItem)
        {
            _context.Update(cartItem);
        }
        public async Task<bool> IsCartHasItems(int cartId, CancellationToken cancellationToken) =>
            await _context.CartItems.AnyAsync(x => x.CartId == cartId, cancellationToken);
    }
}
