using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class UsresController(IUserMangerService userMangerService) : ControllerBase
    {
        private readonly IUserMangerService _userMangerService = userMangerService;

        [HttpGet("")]
        public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
        {
            var result = await _userMangerService.GetInformationAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, cancellationToken);
            return Ok(result.Value);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userMangerService.UpdateAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("change-password")]

        public async Task<IActionResult> ChangePassword(UpdateUserPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _userMangerService.ChangePasswordAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
