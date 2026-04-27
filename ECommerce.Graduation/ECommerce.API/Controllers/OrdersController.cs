using Azure.Core;
using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [Authorize(Roles = "Customer")]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] OrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _orderService.CreateAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request, cancellationToken);
            return result.IsSuccess 
                ? CreatedAtAction(nameof(GetDetails), new { id = result.Value!.OrderId }, result.Value) 
                : result.ToProblem();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _orderService.GetAllAsync(cancellationToken);
            return Ok(result.Value);
        }
    
        
        [Authorize(Roles = "Customer")]
        [HttpGet("")]
        public async Task<IActionResult> GetMyOrders(CancellationToken cancellationToken)
        {
            var result = await _orderService.GetMyOrdersAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetDetailsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, User.FindFirstValue(ClaimTypes.Role)!, id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [Authorize]
        [HttpGet("{id}/track")]
        public async Task<IActionResult> GetTrack([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrderTrackAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, User.FindFirstValue(ClaimTypes.Role)!, id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] int id, [FromBody] UpdateOrederStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _orderService.UpdateStausAsync(id, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancell([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _orderService.CancellAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, User.FindFirstValue(ClaimTypes.Role)!, id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


    }
}
