using Iter.API.Controllers;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Iter.Core.Enum;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class HomeController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IStatisticService homeService, ILogger<HomeController> logger)
        {
            _statisticService = homeService;
            _logger = logger;
        }

        [HttpGet("adminStatistic")]
        public async Task<IActionResult> GetAdminStatistic()
        {
            _logger.LogInformation("GetAdminStatistic operation started.");

            try
            {
                var statistics = await _statisticService.GetAdminStatistic();
                _logger.LogInformation("GetAdminStatistic operation completed successfully.");
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetAdminStatistic operation.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("employeeStatistic")]
        public async Task<IActionResult> GetEmployeeStatistic(string agencyId)
        {
            _logger.LogInformation("GetEmployeeStatistic operation started for agencyId: {AgencyId}", agencyId);

            try
            {
                var statistics = await _statisticService.GetEmployeeStatistic(agencyId);
                _logger.LogInformation("GetEmployeeStatistic operation completed successfully for agencyId: {AgencyId}", agencyId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetEmployeeStatistic operation for agencyId: {AgencyId}", agencyId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
