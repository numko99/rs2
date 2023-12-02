using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Iter.Core.Dto;
using Swashbuckle.AspNetCore.Annotations;
using Iter.Services.Interface;
using Iter.Services;
using Iter.Core.Responses;

namespace Iter.Api.Controllers
{
    [Route("api/userauthentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IUserAuthenticationService userAuthenticationService;
        public AuthController(ILogger<AuthController> logger, IUserAuthenticationService userAuthenticationService)
        {
            this.logger = logger;
            this.userAuthenticationService = userAuthenticationService;
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "User has been succesfully created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with data sent with the request")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await this.userAuthenticationService.RegisterUserAsync(userRegistration);

            if (!userResult.Succeeded)
            {
                this.logger.LogError("Failed to register user: {ErrorMessage}", userResult.Errors.ToString());
                return this.BadRequest(userResult);
            }

            this.logger.LogInformation("User registered successfully");
            return this.StatusCode(201);
        }

        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status200OK, "User has been succesfully authenticated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Problem with data sent with the request")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            if (!await this.userAuthenticationService.ValidateUserAsync(user))
            {
                this.logger.LogWarning("User validation failed for username: {Username}", user.UserName);
                return Unauthorized();
            }

            this.logger.LogInformation("User validated successfully");

            var token = await this.userAuthenticationService.CreateTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            this.logger.LogInformation("Token created successfully");

            return Ok(new LoginResponse() { Token = token });
        }
    }
}
