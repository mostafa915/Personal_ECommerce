using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Users;
using ECommerce.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class UserMangerService(UserManager<ApplicationUser> userManager) : IUserMangerService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<UserResponse>> GetInformationAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return Result.Success(user.Adapt<UserResponse>()!);
        }

        public async Task<Result> UpdateAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user!.FirstName = request.FirstName;
            user.LastName = request.LastName;

          var result =  await _userManager.UpdateAsync(user);
            if(result.Succeeded) 
            return Result.Success();

            var error = result.Errors.First();
            return Result.Faliuar(new Error(error.Code, error.Description, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result> ChangePasswordAsync(string userId, UpdateUserPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user!, request.OldPass, request.NewPass);
            
            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Faliuar(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));

        }
    }
}
