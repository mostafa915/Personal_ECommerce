using Azure.Core;
using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService, IReviewService reviewService) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly IReviewService _reviewService = reviewService;

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId, [FromQuery] int? brandId, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAsync(search, categoryId, brandId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        
        
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAvailable([FromQuery] string? search, [FromQuery] int? categoryId, [FromQuery] int? brandId, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAvailableAsync(search, categoryId, brandId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("{id}/available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailable([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAvailableAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }




        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.CreateAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _productService.ToggleAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> CahngeQuantityAsync([FromRoute] int id, UpdateProductQuatityRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateQuantityAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> ReviewAsync([FromRoute] int id, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
        {
            var result = await _reviewService.ReviewAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();    
        }

        [HttpGet("{id}/reviews")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _reviewService.GetReviewsProductAsync(id, cancellationToken);
            return Ok(result.Value);
        }
    }
}
