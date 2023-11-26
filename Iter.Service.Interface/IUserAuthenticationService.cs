using Iter.Core.Dto;
using Microsoft.AspNetCore.Identity;

namespace Iter.Services.Interface
{
    public interface IUserAuthenticationService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);

        Task<bool> ValidateUserAsync(UserLoginDto loginDto);

        Task<string> CreateTokenAsync();
    }
}