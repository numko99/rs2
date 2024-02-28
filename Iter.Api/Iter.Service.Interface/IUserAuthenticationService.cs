using Iter.Core.Dto;
using Iter.Core.EntityModels;
using Microsoft.AspNetCore.Identity;

namespace Iter.Services.Interface
{
    public interface IUserAuthenticationService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);

        Task<List<string>?> GetUserRoleIdsAsync(User user);

        Task<bool> ValidateUserAsync(UserLoginDto loginDto);

        Task<string> CreateTokenAsync();
    }
}