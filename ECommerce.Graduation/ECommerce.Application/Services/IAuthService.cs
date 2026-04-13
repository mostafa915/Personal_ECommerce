using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IAuthService
    {
        Task<Result<AutheResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AutheResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result<AutheResponse>> RegisterAsync(RegisterationRequest request, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    }
}
