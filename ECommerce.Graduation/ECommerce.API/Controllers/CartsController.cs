using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CartsController(ICartService cartService) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;

        [HttpGet("")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var result = await _cartService.CreatUserCartAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, cancellationToken);
            return Ok(result.Value);
        }

        [HttpPost("/items")]
        public async Task<IActionResult> AddItems([FromBody] CartItemRequest request, CancellationToken cancellationToken)
        {
            var result = await _cartService.AddItemsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request, cancellationToken);
            return result.IsSuccess ? Ok("Item added to cart") : result.ToProblem();
        }
        [HttpPut("/items/{productId}")]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] UpdateCartItemRequest request, CancellationToken cancellationToken)
        {
            var result = await _cartService.UpdateItemAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, productId, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }

        [HttpDelete("items/{productId}")]

        public async Task<IActionResult> Remove([FromRoute] int productId, CancellationToken cancellationToken)
        {
            var result = await _cartService.RemoveItemAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, productId, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("")]
        public async Task<IActionResult> Clear(CancellationToken cancellationToken)
        {
            var result = await _cartService.ClearAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
