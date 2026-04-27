using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Errors;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService(ICartRepo cartRepo, IOrderRepo orderRepo) : IOrderService
    {
        private readonly ICartRepo _cartRepo = cartRepo;
        private readonly IOrderRepo _orderRepo = orderRepo;

        public async Task<Result<OrderResponse>> CreateAsync(string userId, OrderRequest request, CancellationToken cancellationToken = default)
        {
            if (await _cartRepo.GetCartAsync(userId, cancellationToken) is not { } userCart)
                return Result.Faliuar<OrderResponse>(CartsError.NotFound);

            var IsCartHasItems = await _cartRepo.IsCartHasItems(userCart.Id, cancellationToken);
            if (!IsCartHasItems)
                return Result.Faliuar<OrderResponse>(CartsError.NoHasItems);

            var CartItems = await _cartRepo.GetItemsAsync(userCart.Id, cancellationToken);
            decimal total = 0;
            
            foreach (var item in CartItems)
            {
                if (item.Quantity > item.Product.QuantityAvailable)
                    return Result.Faliuar<OrderResponse>(ProductsError.QuantityIsNotEnough);
                total += item.Quantity * item.PriceAtTimeOfAdd;

            }

            var order = new Order
            {
                TotalAmount = total,
                Address = request.Address,
                OrderItems = CartItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Price = x.PriceAtTimeOfAdd,
                    Quantity = x.Quantity,
                })
                .ToList()
            };
            
            

            await _orderRepo.BeginTransactionAsync();

            try
            {
                await _orderRepo.AddAsync(order,cancellationToken);

                foreach (var item in CartItems)
                {
                    item.Product.QuantityAvailable -= item.Quantity;
                }

                _cartRepo.ClearItems(CartItems);

                await _orderRepo.CommitAsync();
            }
            catch
            {
                await _orderRepo.RollbackAsync();
                return Result.Faliuar<OrderResponse>(OrdersError.Failed);
            }
            var response = new OrderResponse(order.Id, order.TotalAmount, order.OrderStatus.ToString(), order.CreatedOn);
            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<OrderAllResonse>>> GetAllAsync(CancellationToken cancellationToken  = default)
        {
            var orders = await _orderRepo.GetAllAsync(cancellationToken);
            return Result.Success(orders);
        }

        public async Task<Result<IEnumerable<OrderResponse>>> GetMyOrdersAsync(string userId, CancellationToken cancellationToken = default)
        {
            var myOrders = await _orderRepo.GetMyOrdersAsync(userId, cancellationToken);
            return Result.Success(myOrders);
        }

        public async Task<Result<OrderDetailsResponse>> GetDetailsAsync(string userId, string role, int id, CancellationToken cancellationToken = default)
        {
            if (await _orderRepo.GetDetailsAsync(id, cancellationToken) is not { } order)
                return Result.Faliuar<OrderDetailsResponse>(OrdersError.NotFound);

            if(role == "Customer" && !(await _orderRepo.IsUserHasThisOrderAsync(userId, id, cancellationToken)))
                return Result.Faliuar<OrderDetailsResponse>(OrdersError.NotFound);

            return Result.Success(order);

        }


        public async Task<Result<OrderTrackResponse>> GetOrderTrackAsync(string userId, string role, int orderId, CancellationToken cancellationToken = default)
        {
            if (await _orderRepo.GetOrderByIdAsync(orderId, cancellationToken) is not { } order)
                return Result.Faliuar<OrderTrackResponse>(OrdersError.NotFound);

            if (role == "Customer" && !(await _orderRepo.IsUserHasThisOrderAsync(userId, orderId, cancellationToken)))
                return Result.Faliuar<OrderTrackResponse>(OrdersError.NotFound);

            var response = new OrderTrackResponse(order.Id, order.OrderStatus.ToString());

            return Result.Success(response);

        }

        public async Task<Result<UpdateOrederStatusResponse>> UpdateStausAsync(int orderId, UpdateOrederStatusRequest request, CancellationToken cancellationToken = default)
        {
            if (await _orderRepo.GetOrderByIdAsync(orderId, cancellationToken) is not { } order)
                return Result.Faliuar<UpdateOrederStatusResponse>(OrdersError.NotFound);

            if (!Enum.IsDefined(typeof(DeliveryTracking), request.Status))
                return Result.Faliuar<UpdateOrederStatusResponse>(OrdersError.StatusNotExists);

            var oldStatus = order.OrderStatus;
            
            var isTransitionAllowed = IsTransitionAllowed(oldStatus, request.Status);
            if (!isTransitionAllowed)
                return Result.Faliuar<UpdateOrederStatusResponse>(OrdersError.InvaildStatusTranstion);

            var response = new UpdateOrederStatusResponse(order.Id, oldStatus.ToString(), request.Status.ToString());

            order.OrderStatus = request.Status;
            var orderItems = await _orderRepo.GetOrderItemsAsync(orderId, cancellationToken);
            if(request.Status == DeliveryTracking.Cancelled)
            {
                foreach (var item in orderItems)
                {
                    item.Product.QuantityAvailable += item.Quantity;
                }
            }

            await _orderRepo.SaveChangesAsync(cancellationToken);

            return Result.Success(response);
        }



        public async Task<Result> CancellAsync(string userId, string role, int orderId, CancellationToken cancellationToken = default)
        {
            if (await _orderRepo.GetOrderByIdAsync(orderId, cancellationToken) is not { } order)
                return Result.Faliuar(OrdersError.NotFound);

            if (role == "Customer" && !(await _orderRepo.IsUserHasThisOrderAsync(userId, orderId, cancellationToken)))
                return Result.Faliuar(OrdersError.NotFound);


            var oldStatus = order.OrderStatus;
           
            if(oldStatus == DeliveryTracking.Delivered || oldStatus == DeliveryTracking.Cancelled || oldStatus == DeliveryTracking.Shipped)
                return Result.Faliuar(OrdersError.InvaildStatusTranstion);


            order.OrderStatus = DeliveryTracking.Cancelled;
            
            var orderItems = await _orderRepo.GetOrderItemsAsync(orderId, cancellationToken);
            foreach( var item in orderItems)
            {
                item.Product.QuantityAvailable += item.Quantity;
            }
            
            await _orderRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }

        

        private bool IsTransitionAllowed(DeliveryTracking oldStatus, DeliveryTracking newStatus)
        {
            if (((int)oldStatus + 1 != (int)newStatus) && newStatus != DeliveryTracking.Cancelled)
                return false;

            if (((oldStatus == DeliveryTracking.Delivered || oldStatus == DeliveryTracking.Shipped) && newStatus == DeliveryTracking.Cancelled))
                return false;

            if ((oldStatus == newStatus))
                return false;

            if (oldStatus == DeliveryTracking.Cancelled && newStatus != DeliveryTracking.Cancelled)
                return false;
            
            return true;
        }
    }
}
