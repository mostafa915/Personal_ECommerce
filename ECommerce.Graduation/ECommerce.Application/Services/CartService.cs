using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.Errors;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CartService(UserManager<ApplicationUser> userManager, ICartRepo cartRepo, IProductRepo productRepo) : ICartService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICartRepo _cartRepo = cartRepo;
        private readonly IProductRepo _productRepo = productRepo;

        public async Task<Result<CartResponse>> CreatUserCartAsync(string UserId, CancellationToken cancellationToken)
        {
            if(await _cartRepo.GetCartAsync(UserId, cancellationToken) is not { } cart)
            {
             cart = new Cart
            {
                UserId = UserId,
            };
            await _cartRepo.AddAsync(cart, cancellationToken);
            await _cartRepo.SavaChangesAsync(cancellationToken);
            }
            var cartItems = await _cartRepo.GetItemsAsync(cart.Id, cancellationToken);
            decimal totalPrice = 0;

            List<CartItemResponse> cartItemsResponse = [];
            foreach (var cartItem in cartItems)
            {
                var total = cartItem.Quantity * cartItem.PriceAtTimeOfAdd;
                cartItemsResponse.Add(new CartItemResponse(
                    cartItem.ProductId,
                    cartItem.Product.Name,
                    cartItem.Product.Price,
                    cartItem.Quantity,
                    total
                    ));
                totalPrice += total;
            }

            var response = new CartResponse
                (
                    cart.Id,
                    cartItemsResponse,
                    totalPrice
                );

            return Result.Success(response);
        }

        public async Task <Result> AddItemsAsync(string userId, CartItemRequest request, CancellationToken cancellationToken)
        {
            if (await _cartRepo.GetCartAsync(userId, cancellationToken) is not { } cart)
                return Result.Faliuar(CartsError.NotFound);

            if (await _productRepo.GetAvailableByIdAsync(request.ProductId, cancellationToken) is not { } product)
                return Result.Faliuar(ProductsError.NotFound);

            if(request.Quantity > product.QuantityAvailable)
                return Result.Faliuar(ProductsError.QuantityIsNotEnough);

            var cartItem = await _cartRepo.GetItemByProductIdAsync(cart.Id, product.Id, cancellationToken);
            if(cartItem is not null)
            {
                if ((request.Quantity + cartItem.Quantity) > product.QuantityAvailable)
                {
                    return Result.Faliuar(ProductsError.QuantityIsNotEnough);
                }

                cartItem.Quantity += request.Quantity;
                _cartRepo.Update(cartItem);
                await _cartRepo.SavaChangesAsync(cancellationToken);
                return Result.Success();
            }
            cartItem = new CartItem
            {
                CartId = cart.Id,
                Quantity = request.Quantity,
                ProductId = product.Id,
                PriceAtTimeOfAdd = product.Price
            };

            await _cartRepo.AddAsync(cartItem, cancellationToken);
            await _cartRepo.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> UpdateItemAsync(string userId, int productId, UpdateCartItemRequest request, CancellationToken cancellationToken)
        {
            if (await _cartRepo.GetCartAsync(userId, cancellationToken) is not { } cart)
                return Result.Faliuar(CartsError.NotFound);

            if (await _productRepo.GetAvailableByIdAsync(productId, cancellationToken) is not { } product)
                return Result.Faliuar(ProductsError.NotFound);

            if (request.Quantity > product.QuantityAvailable)
                return Result.Faliuar(ProductsError.QuantityIsNotEnough);

            var cartItem = await _cartRepo.GetItemByProductIdAsync(cart.Id, product.Id, cancellationToken);
            if (cartItem is null)
                return Result.Faliuar(CartsError.ItemNotFound);

            cartItem!.Quantity = request.Quantity;
            await _cartRepo.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result> RemoveItemAsync(string userId, int productId, CancellationToken cancellationToken)
        {
            if (await _cartRepo.GetCartAsync(userId, cancellationToken) is not { } cart)
                return Result.Faliuar(CartsError.NotFound);

            if (await _productRepo.GetAvailableByIdAsync(productId, cancellationToken) is not { } product)
                return Result.Faliuar(ProductsError.NotFound);

            var cartItem = await _cartRepo.GetItemByProductIdAsync(cart.Id, product.Id, cancellationToken);
            if (cartItem is null)
                return Result.Faliuar(CartsError.ItemNotFound);

            _cartRepo.Remove(cartItem);
            await _cartRepo.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result> ClearAsync(string userId, CancellationToken cancellationToken)
        {
            if (await _cartRepo.GetCartAsync(userId, cancellationToken) is not { } cart)
                return Result.Faliuar(CartsError.NotFound);
            
            var cartItems = await _cartRepo.GetItemsAsync(cart.Id, cancellationToken);
            _cartRepo.ClearItems(cartItems);
            await _cartRepo.SavaChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
