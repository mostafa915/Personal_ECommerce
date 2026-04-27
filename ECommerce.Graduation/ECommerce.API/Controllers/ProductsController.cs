using Azure.Core;
using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId, [FromQuery] int? brandId, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAsync(search, categoryId, brandId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

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


        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.CreateAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }
        
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _productService.ToggleAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> CahngeQuantityAsync([FromRoute] int id, UpdateProductQuatityRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateQuantityAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
