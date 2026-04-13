using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;


        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();  
        }

        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvaliable(CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAllAvailableAsync(cancellationToken);
            return Ok(result.Value);
        }

        [HttpGet("{id}/available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvaliable([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAvaliableAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.CreateAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> Toggle([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.ToggleAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


    }
}
