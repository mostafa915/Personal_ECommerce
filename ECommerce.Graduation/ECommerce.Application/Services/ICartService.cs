using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface ICartService
    {
        Task<Result<CartResponse>> CreatUserCartAsync(string UserId, CancellationToken cancellationToken);
        Task<Result> AddItemsAsync(string userId, CartItemRequest request, CancellationToken cancellationToken);
        Task<Result> UpdateItemAsync(string userId, int productId, UpdateCartItemRequest request, CancellationToken cancellationToken);
        Task<Result> RemoveItemAsync(string userId, int productId, CancellationToken cancellationToken);
        Task<Result> ClearAsync(string userId, CancellationToken cancellationToken);
    }
}
