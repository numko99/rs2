using AutoMapper;
using Iter.Core;
using Iter.Core.Dto;
using Iter.Core.EntityModels;
using Iter.Core.EntityModelss;
using Iter.Core.Enum;
using Iter.Core.Helper;
using Iter.Core.Options;
using Iter.Core.Requests;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Reporting.Map.WebForms.BingMaps;
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
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;
        private readonly IVerificationTokenRepository verificationTokenRepository;

        private User? user;

        public UserAuthenticationService(
        UserManager<User> userManager, IMapper mapper, JwtConfiguration jwtConfiguration, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IVerificationTokenRepository verificationTokenRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.jwtConfiguration = jwtConfiguration;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
            this.verificationTokenRepository = verificationTokenRepository;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var existingUser = await userManager.FindByEmailAsync(userRegistration.Email);

            if (existingUser == null)
            {
                var user = mapper.Map<User>(userRegistration);
                user.Role = (int)Roles.Client;
                var result = await userManager.CreateAsync(user, userRegistration.Password);
                await this.userManager.AddToRoleAsync(user, Roles.Client.ToString());
                if (result.Succeeded)
                {
                    await this.CreateAndSendToken(user, EmailHelper.EMAIL_VERIFICATION);
                }
                return result;
            }
            else if (!existingUser.EmailConfirmed)
            {
                existingUser.PasswordHash = userManager.PasswordHasher.HashPassword(existingUser, userRegistration.Password);
                existingUser.PhoneNumber = userRegistration.PhoneNumber;
                var updateResult = await userManager.UpdateAsync(existingUser);

                if (updateResult.Succeeded)
                {
                    await this.CreateAndSendToken(existingUser, EmailHelper.EMAIL_VERIFICATION);
                }
                return updateResult;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Email '{existingUser.Email}' is already taken.", Code = "DuplicateEmail" });
            }
        }

        public async Task CreateClient(UserRegistrationDto userRegistration)
        {
            var user = await userManager.FindByEmailAsync(userRegistration.Email);

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


        public async Task<User> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<User?> ValidateUserAsync(UserLoginDto loginDto)
        {
            this.user = await this.userManager.FindByNameAsync(loginDto.UserName);

            if (this.user != null &&  (await this.userManager.CheckPasswordAsync(this.user, loginDto.Password) && user.EmailConfirmed))
            {
                return this.user;
            }

            return null;
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

        public async Task<User> GetCurrentUserAsync()
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new InvalidOperationException("No logged-in user.");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return user;
        }


        public async Task<bool> IsCorrectPassword(string password)
        {
            var currentUser = httpContextAccessor.HttpContext?.User;
            var user = await userManager.GetUserAsync(currentUser);

            if (user == null)
            {
                return false;
            }


            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task ChangePassword(string currentPassword, string newPassword)
        {
            var currentUser = httpContextAccessor.HttpContext?.User;
            var user = await userManager.GetUserAsync(currentUser);

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        }


        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return;
            }

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, request.Password);
            await userManager.UpdateAsync(user);
        }

        public async Task<bool> IsValidVerificationToken(string? email, string? token)
        {
            var user = await userManager.FindByEmailAsync(email);

            var verificationToken = await this.verificationTokenRepository.GetLastTokenByUserId(user.Id);

            if (verificationToken == null)
            {
                return false;
            }

            if (verificationToken.Token == token && verificationToken.ExpiryDate > DateTime.Now)
            {
                verificationToken.ExpiryDate = DateTime.Now;
                await this.verificationTokenRepository.UpdateAsync(verificationToken);

                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                return true;
            }

            return false;
        }

        public async Task ResendToken(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            var verificationToken = await this.verificationTokenRepository.GetLastTokenByUserId(user.Id);
            if (verificationToken == null)
            {
                return;
            }

            if(verificationToken.ExpiryDate > DateTime.Now)
            {
                verificationToken.ExpiryDate = DateTime.Now;
                await this.verificationTokenRepository.UpdateAsync(verificationToken);
            }

            await this.CreateAndSendToken(user, EmailHelper.EMAIL_VERIFICATION);
        }


        public async Task CreateAndSendToken(User user, string type)
        {
            Random rnd = new Random();
            int code = rnd.Next(1000, 9999);

            await this.verificationTokenRepository.AddAsync(new VerificationToken()
            {
                Token = code.ToString(),
                UserId = user.Id,
                VerificationTokenType = type,
                ExpiryDate = DateTime.Now.AddHours(1),
            });

            if (type == EmailHelper.EMAIL_VERIFICATION)
            {
                await emailService.SendEmailAsync(
                    new Core.Models.EmailMessage()
                    {
                        Email = user.Email,
                        Subject = "Verifikacija korisničkog računa",
                        Content = "<p>Hvala vam što ste se registrirali na našu aplikaciju. Kako bismo dovršili vašu registraciju, potrebno je verificirati vašu e-mail adresu.</p>" +
                                    "<p>Molimo vas da unesete sljedeći verifikacijski kod u predviđeno polje na aplikaciji:</p>" +
                                    $"<p style='margin-top:0px'>Verifikacijski kod: <b>{code}</b></p>" +
                                    "<p>Ovaj kod je važeći 60 minuta od trenutka kada je poslan. Verifikacijom vaše e-mail adrese osiguravate sigurnost vašeg računa i omogućavate puni pristup svim funkcijama platforme.</p>"
                    });
            }


            if (type == EmailHelper.FORGOT_PASSWORD)
            {
                await emailService.SendEmailAsync(new Core.Models.EmailMessage()
                {
                    Email = user.Email,
                    Subject = "Reset lozinke",
                    Content = "<p>Primili smo zahtjev za resetiranje vaše lozinke na našoj platformi. Ukoliko niste zatražili promjenu lozinke, molimo vas da zanemarite ovu poruku.</p>" +
                    "<p>Ako želite resetirati lozinku, molimo vas da unesete sljedeći kod za verifikaciju na aplikaciji za resetiranje lozinke:</p>" +
                    $"<p style='margin-top:0px'>Verifikacijski kod: <b>{code}</b></p>" +
                    "<p>Ovaj kod je važeći 60 minuta od trenutka kada je poslan. Upisom ovog koda na našoj web stranici moći ćete postaviti novu lozinku.</p>"
                });
            }
        }
    }
}