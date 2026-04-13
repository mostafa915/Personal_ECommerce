using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("")]
        public async Task<IActionResult> LogIn([FromBody] AuthRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

            return result.IsSuccess ? Ok(result.Value)
                : result.ToProblem();

        }
        [HttpPost("refresh")]

        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ? Ok(result.Value)
                : result.ToProblem();

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();

        }

        [HttpPut("revoke-refresh-token")]

        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var reasult = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return !reasult ? BadRequest("Opearation Failed") : NoContent();
        }
    }
}
