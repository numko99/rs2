using Iter.Core.Dto;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Microsoft.AspNetCore.Identity;

namespace Iter.Services.Interface
{
    public interface IUserAuthenticationService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);

        Task<List<string>?> GetUserRoleIdsAsync(User user);

        Task<User?> ValidateUserAsync(UserLoginDto loginDto);

        Task<bool> IsValidVerificationToken(string? email, string? token);

        Task ResendToken(string email);

        Task<User> GetUserByEmail(string email);

        Task<string> CreateTokenAsync();

        Task<User> GetCurrentUserAsync();

        Task<bool> IsCorrectPassword(string password);

        Task ResetPassword(ResetPasswordRequest userRegistrationDto);

        Task ChangePassword(string currentPassword, string newPassword);

        Task CreateAndSendToken(User user, string type);

        Task<string> GetCurrentUserIdAsync();
    }
}