using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IRepos
{
    public interface IOrderRepo
    {
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task AddAsync(OrderItem item, CancellationToken cancellationToken);
        Task<IEnumerable<OrderResponse>> GetMyOrdersAsync(string userId, CancellationToken cancellationToken);
        Task<OrderDetailsResponse?> GetDetailsAsync(int orderId, CancellationToken cancellationToken);
        Task<bool> IsUserHasThisOrderAsync(string userId, int orderId, CancellationToken cancellationToken);
        Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId, CancellationToken cancellationToken);
        Task<IEnumerable<OrderAllResonse>> GetAllAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
