using Iter.API.Controllers;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core.EntityModels;
using Iter.Core;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseCRUDController<User, UserUpsertRequest, UserUpsertRequest, UserResponse, UserSearchModel>
    {
        private readonly IUserService _userService;
        public UserController(IUserService clientService, IUserService userService) : base(clientService)
        {
            _userService = userService;
        }

        [HttpPut("new-password/{id}")]
        public virtual async Task<IActionResult> NewPassword(string id)
        {
            await this._userService.NewPassword(id);
            return Ok();
        }

    }
}