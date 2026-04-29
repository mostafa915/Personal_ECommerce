using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IUserMangerService
    {
        Task<Result<UserResponse>> GetInformationAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> ChangePasswordAsync(string userId, UpdateUserPasswordRequest request, CancellationToken cancellationToken = default);
    }
}
