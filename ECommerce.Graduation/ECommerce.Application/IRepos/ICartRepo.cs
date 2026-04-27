using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IRepos
{
    public interface ICartRepo
    {
        Task AddAsync(Cart cart, CancellationToken cancellationToken);
        Task AddAsync(CartItem itme, CancellationToken cancellationToken);
        Task<IEnumerable<CartItem>> GetItemsAsync(int cartId, CancellationToken cancellationToken);
        Task<int> SavaChangesAsync(CancellationToken cancellationToken);
        Task<Cart?> GetCartAsync(string userId, CancellationToken cancellationToken);
        void Update(Cart cart);
        void Update(CartItem cartItem);
        Task<CartItem?> GetItemByProductIdAsync(int cartId ,int productId,CancellationToken cancellationToken);
        void Remove(CartItem cartItem);
        void ClearItems(IEnumerable<CartItem> items);
        Task<bool> IsCartHasItems(int cartId, CancellationToken cancellationToken);
    }
}
