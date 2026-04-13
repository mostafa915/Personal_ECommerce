using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Brands;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BrandsController(IBrandService brandService) : ControllerBase
    {
        private readonly IBrandService _brandService = brandService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _brandService.GetAllAsync(cancellationToken);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _brandService.GetAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvaliable(CancellationToken cancellationToken)
        {
            var result = await _brandService.GetAllAvailableAsync(cancellationToken);
            return Ok(result.Value);
        }

        [HttpGet("{id}/available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvaliable([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _brandService.GetAvailableAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }



        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] BrandRequest request, CancellationToken cancellationToken)
        {
            var result = await _brandService.CreatAsync(request, cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]  BrandRequest request, CancellationToken cancellationToken)
        {
            var result = await _brandService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> Toggle([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _brandService.ToggleAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
