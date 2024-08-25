using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Iter.Core.Search_Models;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Iter.Model;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide))]
    public class EmployeeArrangmentController : BaseCRUDController<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>
    {
        private readonly IEmployeeArrangmentService _employeeArrangmentService;
        private readonly ILogger<EmployeeArrangmentController> _logger;

        public EmployeeArrangmentController(IEmployeeArrangmentService employeearrangmentService, IEmployeeArrangmentService employeeArrangmentService, ILogger<EmployeeArrangmentController> logger)
            : base(employeearrangmentService, logger)
        {
            _employeeArrangmentService = employeeArrangmentService;
            _logger = logger;
        }

        [HttpGet("available-guides")]
        public async Task<IActionResult> GetAvailableGuides([FromQuery] string arrangementId, [FromQuery] string dateFrom, [FromQuery] string? dateTo)
        {
            _logger.LogInformation("GetAvailableGuides operation started with parameters: arrangementId={ArrangementId}, dateFrom={DateFrom}, dateTo={DateTo}", arrangementId, dateFrom, dateTo);

            try
            {
                var availableGuides = await _employeeArrangmentService.GetAvailableEmployeeArrangmentsAsync(new Guid(arrangementId), dateFrom, dateTo);
                _logger.LogInformation("GetAvailableGuides operation completed successfully with {Count} results.", availableGuides?.Count ?? 0);
                return Ok(availableGuides);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetAvailableGuides operation with parameters: arrangementId={ArrangementId}, dateFrom={DateFrom}, dateTo={DateTo}", arrangementId, dateFrom, dateTo);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
