using AutoMapper;
using Iter.Core;
using Iter.Core.Dto;
using Iter.Core.EntityModels;
using Iter.Core.Options;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Iter.Services
{
    public sealed class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly JwtConfiguration jwtConfiguration;

        private User? user;

        public UserAuthenticationService(
        UserManager<User> userManager, IMapper mapper, JwtConfiguration jwtConfiguration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.jwtConfiguration = jwtConfiguration;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = mapper.Map<User>(userRegistration);
            var result = await userManager.CreateAsync(user, userRegistration.Password);
            return result;
        }

        public async Task<List<string>?> GetUserRoleIdsAsync(User user)
        {
            if (user == null)
            {
                return null;
            }

            var roleNames = await this.userManager.GetRolesAsync(user);
            var roleIds = new List<string>();

            foreach (var roleName in roleNames)
            {
                var role = await this.roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roleIds.Add(role.Id);
                }
            }

            return roleIds;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto loginDto)
        {
            this.user = await this.userManager.FindByNameAsync(loginDto.UserName);
            var result = this.user != null && await this.userManager.CheckPasswordAsync(this.user, loginDto.Password);
            return result;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials? GetSigningCredentials()
        {
            if (this.jwtConfiguration?.Secret == null)
                return null;

            var key = Encoding.UTF8.GetBytes(this.jwtConfiguration.Secret);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await this.userManager.GetRolesAsync(this.user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
            issuer: this.jwtConfiguration.ValidIssuer,
            audience: this.jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfiguration.ExpiresIn)),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}