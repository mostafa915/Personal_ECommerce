using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repos
{
    public class OrderRepo(ApplicationDbContext context) : IOrderRepo
    {
        private readonly ApplicationDbContext _context = context;
        private IDbContextTransaction _transaction;

        public async Task AddAsync(Order order, CancellationToken cancellationToken) =>
            await _context.AddAsync(order, cancellationToken);

        public async Task AddAsync(OrderItem item, CancellationToken cancellationToken) =>
            await _context.AddAsync(item, cancellationToken);


        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }


        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId, CancellationToken cancellationToken) =>
            await _context.OrderItems
            .Where(x => x.OrderId == orderId)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);
        public async Task<OrderDetailsResponse?> GetDetailsAsync(int orderId, CancellationToken cancellationToken) =>
            await _context.Orders
            .Where(x => x.Id == orderId)
            .Include(x => x.OrderItems)
            .Select(x => new OrderDetailsResponse
            (
                x.Id,
                x.OrderStatus.ToString(),
                x.TotalAmount,
                x.OrderItems.Select(x => new OrderItemResponse(x.ProductId, x.Product.Name, x.Price, x.Quantity))
                .ToList()
            ))
            .SingleOrDefaultAsync(cancellationToken);


        public async Task<IEnumerable<OrderAllResonse>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.Orders
            .Include(x => x.OrderItems)
            .Select(x => new OrderAllResonse(
                $"{x.CreatedBy.FirstName} {x.CreatedBy.LastName}",
                 x.Id,
                 x.OrderStatus.ToString(),
                 x.TotalAmount,
                 x.OrderItems.Select(x => new OrderItemResponse(x.ProductId, x.Product.Name, x.Price, x.Quantity))
                .ToList()))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
       
        public async Task<IEnumerable<OrderResponse>> GetMyOrdersAsync(string userId, CancellationToken cancellationToken) =>
            await _context.Orders
            .Where(x => x.CreatedById == userId)
            .Select(x => new OrderResponse(x.Id, x.TotalAmount, x.OrderStatus.ToString(), x.CreatedOn))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        public async Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken) =>
            await _context.Orders.SingleOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        public async Task<bool> IsUserHasThisOrderAsync(string userId, int orderId, CancellationToken cancellationToken) =>
            await _context.Orders.AnyAsync(x => x.Id == orderId && x.CreatedById == userId, cancellationToken);

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
