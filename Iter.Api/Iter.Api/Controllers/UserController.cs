using Iter.API.Controllers;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Iter.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Iter.Core.Enum;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseCRUDController<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel, UserResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserAuthenticationService userAuthenticationService;

        public UserController(IUserService userService, IUserAuthenticationService userAuthenticationService) : base(userService)
        {
            _userService = userService;
            this.userAuthenticationService = userAuthenticationService;
        }

        [HttpPut("new-password/{id}")]
        public virtual async Task<IActionResult> NewPassword(string id)
        {
            await this._userService.NewPassword(id);
            return Ok();
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var checkPassword = await userAuthenticationService.IsCorrectPassword(request.CurrentPassword);
            if (!checkPassword)
            {
                return Unauthorized("Current password is incorrect");
            }

            await userAuthenticationService.ChangePassword(request.CurrentPassword, request.NewPassword);
            return Ok();
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await this._userService.GetById(new Guid(userId)));
        }

        [HttpGet("current-user-statistic-data")]
        [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.TouristGuide))]
        public async Task<IActionResult> GetCurrentUserStatisticData()
        {
            return Ok(await this._userService.GetCurrentUserStatistic());
        }


        [HttpPost("currentUser")]
        [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.TouristGuide))]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] CurrentUserUpsertRequest request)
        {
            await this._userService.UpdateCurrentUser(request);
            return Ok();
        }

        [HttpPost("names-by-id")]
        public async Task<IActionResult> GetUserNamesByIds([FromBody] List<string> userIds)
        {
            if (userIds == null || userIds.Count == 0)
            {
                return BadRequest("User IDs are required.");
            }

            var userNames = await _userService.GetUserNamesByIds(userIds);
            return Ok(userNames);
        }
    }
}