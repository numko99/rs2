using Iter.API.Controllers;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.EntityModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Iter.Core.Enum;
using Iter.Model;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide))]
    public class UserController : BaseCRUDController<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IUserAuthenticationService userAuthenticationService, ILogger<UserController> logger) : base(userService, logger)
        {
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
            _logger = logger;
        }

        [HttpPut("new-password/{id}")]
        public virtual async Task<IActionResult> NewPassword(string id)
        {
            _logger.LogInformation("NewPassword operation started for user ID: {Id}", id);

            try
            {
                await _userService.NewPassword(id);
                _logger.LogInformation("NewPassword operation completed successfully for user ID: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during NewPassword operation for user ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            _logger.LogInformation("UpdatePassword operation started for user ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var checkPassword = await _userAuthenticationService.IsCorrectPassword(request.CurrentPassword);
                if (!checkPassword)
                {
                    _logger.LogWarning("UpdatePassword operation failed: current password is incorrect for user ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                    return Unauthorized("Current password is incorrect");
                }

                await _userAuthenticationService.ChangePassword(request.CurrentPassword, request.NewPassword);
                _logger.LogInformation("UpdatePassword operation completed successfully for user ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during UpdatePassword operation for user ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("GetCurrentUser operation started for user ID: {UserId}", userId);

            try
            {
                var user = await _userService.GetById(new Guid(userId));
                _logger.LogInformation("GetCurrentUser operation completed successfully for user ID: {UserId}", userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetCurrentUser operation for user ID: {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("current-user-statistic-data")]
        [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.TouristGuide))]
        public async Task<IActionResult> GetCurrentUserStatisticData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("GetCurrentUserStatisticData operation started for user ID: {UserId}", userId);

            try
            {
                var statisticData = await _userService.GetCurrentUserStatistic();
                _logger.LogInformation("GetCurrentUserStatisticData operation completed successfully for user ID: {UserId}", userId);
                return Ok(statisticData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetCurrentUserStatisticData operation for user ID: {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("currentUser")]
        [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.TouristGuide))]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] CurrentUserUpsertRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("UpdateCurrentUser operation started for user ID: {UserId} with request data: {@Request}", userId, request);

            try
            {
                await _userService.UpdateCurrentUser(request);
                _logger.LogInformation("UpdateCurrentUser operation completed successfully for user ID: {UserId}", userId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during UpdateCurrentUser operation for user ID: {UserId} with request data: {@Request}", userId, request);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("names-by-id")]
        public async Task<IActionResult> GetUserNamesByIds([FromBody] List<string> userIds)
        {
            _logger.LogInformation("GetUserNamesByIds operation started for user IDs: {@UserIds}", userIds);

            if (userIds == null || userIds.Count == 0)
            {
                _logger.LogWarning("GetUserNamesByIds operation failed: no user IDs provided.");
                return BadRequest("User IDs are required.");
            }

            try
            {
                var userNames = await _userService.GetUserNamesByIds(userIds);
                _logger.LogInformation("GetUserNamesByIds operation completed successfully for user IDs: {@UserIds}", userIds);
                return Ok(userNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetUserNamesByIds operation for user IDs: {@UserIds}", userIds);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
