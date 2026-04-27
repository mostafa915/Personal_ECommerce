using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IOrderService
    {
        Task<Result<OrderResponse>> CreateAsync(string userId, OrderRequest request, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<OrderAllResonse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<OrderTrackResponse>> GetOrderTrackAsync(string userId, string role, int orderId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<OrderResponse>>> GetMyOrdersAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<OrderDetailsResponse>> GetDetailsAsync(string userId, string role, int id, CancellationToken cancellationToken = default);
        Task<Result> CancellAsync(string userId, string role, int orderId, CancellationToken cancellationToken = default);
        Task<Result<UpdateOrederStatusResponse>> UpdateStausAsync(int orderId, UpdateOrederStatusRequest request, CancellationToken cancellationToken = default);
    }
}
