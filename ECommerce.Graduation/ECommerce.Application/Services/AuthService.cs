using ECommerce.Application.Abstractions;
using ECommerce.Application.Authentication;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Errors;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwt jwt, IAuthRepo authRepo) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwt _jwt = jwt;
        private readonly IAuthRepo _authRepo = authRepo;
        private readonly int _refreshTokenExpireDays = 15;

        public async Task<Result<AutheResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            //Check If User Is Exist

            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Faliuar<AutheResponse>(UsersError.InValidCrendtial);

            //Check Password

            var IsPasswordConfirmed = await _userManager.CheckPasswordAsync(user, password);

            if (!IsPasswordConfirmed)
                return Result.Faliuar<AutheResponse>(UsersError.InValidCrendtial);

            var roles = await GetUserRolesAsync(user);

            var (token, ExpireIn) = _jwt.GenerateToken(user, roles);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpire = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpireOn = refreshTokenExpire,
            });

            await _userManager.UpdateAsync(user);

            var response = new AutheResponse(user.Id, user.Email, user.FirstName, user.LastName, token, ExpireIn * 60, refreshToken, refreshTokenExpire);
            return Result.Success(response);
        }

        public async Task<Result<AutheResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwt.ValidateToken(token);

            if (userId is null)
                return Result.Faliuar<AutheResponse>(UsersError.NotFound);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Faliuar<AutheResponse>(UsersError.NotFound);


            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);

            if (oldRefreshToken is null)
                return Result.Faliuar<AutheResponse>(UsersError.InValidCrendtial);

            oldRefreshToken.RevokeOn = DateTime.UtcNow;

            var roles = await GetUserRolesAsync(user);

            var (newToken, ExpireIn) = _jwt.GenerateToken(user, roles);

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpire = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpireOn = refreshTokenExpire,
            });
            await _userManager.UpdateAsync(user);
            var response = new AutheResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, ExpireIn * 60, newRefreshToken, refreshTokenExpire);
            return Result.Success(response);
        }


        public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwt.ValidateToken(token);
            if (userId is null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);

            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokeOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }


        public async Task<Result<AutheResponse>> RegisterAsync(RegisterationRequest request, CancellationToken cancellationToken = default)
        {
            var EmailIsAlreadyExists = await _authRepo.AnyEmailAsync(request.Email, cancellationToken);

            if (EmailIsAlreadyExists)
                return Result.Faliuar<AutheResponse>(UsersError.EmailIsExistAlready);

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                var roles = await GetUserRolesAsync(user);

                var (token, ExpireIn) = _jwt.GenerateToken(user, roles);

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpire = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpireOn = refreshTokenExpire,
                });



                await _userManager.UpdateAsync(user);

                var response = new AutheResponse(user.Id, user.Email, user.FirstName, user.LastName, token, ExpireIn * 60, refreshToken, refreshTokenExpire);
                return Result.Success(response);
            }

            var error = result.Errors.First();
            return Result.Faliuar<AutheResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }


        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


        private async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);


            return userRoles;
        }
    }
}
