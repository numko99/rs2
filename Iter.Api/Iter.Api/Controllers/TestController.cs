using Iter.Core;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public TestController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Administrator))]
        
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            return this.Ok();
        }
    }
}
