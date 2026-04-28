using ECommerce.Application.Abstractions;
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
    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _reviewService.RemoveAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!, User.FindFirstValue(ClaimTypes.Role)!, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, ReviewRequest request, CancellationToken cancellationToken)
        {
            var result = await _reviewService.UpdateAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem(); 
        }
    }

}
