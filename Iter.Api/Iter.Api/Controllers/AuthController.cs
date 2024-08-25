using Microsoft.AspNetCore.Mvc;

using Iter.Core.Dto;
using Swashbuckle.AspNetCore.Annotations;
using Iter.Services.Interface;
using Iter.Model;
using Iter.Core.Helper;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly IUserService userService;
        private readonly IAgencyService agencyService;
        public AuthController(ILogger<AuthController> logger, IUserAuthenticationService userAuthenticationService, IUserService userService, IAgencyService agencyService)
        {
            this.logger = logger;
            this.userAuthenticationService = userAuthenticationService;
            this.userService = userService;
            this.agencyService = agencyService;
        }

        [HttpPost("register")]
        [SwaggerResponse(StatusCodes.Status201Created, "User has been succesfully created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with data sent with the request")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await this.userAuthenticationService.RegisterUserAsync(userRegistration);

            if (!userResult.Succeeded)
            {
                this.logger.LogError("Failed to register user: {ErrorMessage}", userResult.Errors.ToString());

                var statusCode = userResult.Errors.Any(x => x.Code == "DuplicateEmail") ? 409 : 400;
                return this.StatusCode(statusCode, userResult);
            }

            this.logger.LogInformation("User registered successfully");
            return this.StatusCode(201);
        }

        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status200OK, "User has been succesfully authenticated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with data sent with the request")]
        public async Task<IActionResult> Authenticate(UserLoginDto user)
        {
            var result = await this.userAuthenticationService.ValidateUserAsync(user);
            if (result == null)
            {
                this.logger.LogWarning("User validation failed for username: {Username}", user.UserName);
                return Unauthorized();
            }

            this.logger.LogInformation("User validated successfully");

            var token = await this.userAuthenticationService.CreateTokenAsync();
            string agencyId = string.Empty;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            if (result.EmployeeId != null)
            {
                var agency = await this.agencyService.GetByEmployeeId(result.EmployeeId.Value);

                if (agency != null)
                {
                    agencyId = agency.Id.ToString();
                }
            }

            this.logger.LogInformation("Token created successfully");
            return Ok(new LoginResponse() { Token = token, Role = result.Role, AgencyId = agencyId, Id = result.Id });
        }

        [HttpPost("verify-email-token")]
        public async Task<IActionResult> VerifyEmailVerificationToken([FromBody] UserRegistrationDto userRegistration)
        {
            if (!await this.userAuthenticationService.IsValidVerificationToken(userRegistration?.Email, userRegistration?.Token))
            {
                this.logger.LogWarning("User verification failed for username: {email}, invalid token", userRegistration.Email);
                return BadRequest();
            }

            await this.userService.InsertClient(userRegistration);

            this.logger.LogInformation("User verification was successfully");


            return Ok();
        }

        [HttpGet("send-forgot-password-token")]
        public async Task<IActionResult> SendForgotPasswordVerificationToken([FromQuery] string email)
        {
            var user = await this.userAuthenticationService.GetUserByEmail(email);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.userAuthenticationService.CreateAndSendToken(user, EmailHelper.FORGOT_PASSWORD);

            return Ok();
        }

        [HttpGet("verify-forgot-password-token")]
        public async Task<IActionResult> VerifyForgotPasswordVerificationToken([FromQuery] string email,[FromQuery] string token)
        {
            if (!await this.userAuthenticationService.IsValidVerificationToken(email, token))
            {
                this.logger.LogWarning("Forgot password verification failed for username: {email}, invalid token", email);
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest userRegistration)
        {
            await this.userAuthenticationService.ResetPassword(userRegistration);
            return Ok();
        }


        [HttpGet("resend-token")]
        public async Task<IActionResult> ResendToken([FromQuery] string email)
        {
            await this.userAuthenticationService.ResendToken(email);
            return Ok();
        }
    }
}
