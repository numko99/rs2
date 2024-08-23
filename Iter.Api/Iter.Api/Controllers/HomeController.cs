using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Microsoft.AspNetCore.Authorization;
using Iter.Core.Enum;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class HomeController: ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public HomeController(IStatisticService homeService)
        {
            _statisticService = homeService;
        }

        [HttpGet("adminStatistic")]
        public async Task<IActionResult> GetAdminStatistic()
        {
            return Ok(await this._statisticService.GetAdminStatistic());
        }

        [HttpGet("employeeStatistic")]
        public async Task<IActionResult> GetEmployeeStatistic(string agencyId)
        {
            return Ok(await this._statisticService.GetEmployeeStatistic(agencyId));
        }


    }
}