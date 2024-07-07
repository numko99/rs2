using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide))]
    public class EmployeeArrangmentController : BaseCRUDController<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>
    {
        private readonly IEmployeeArrangmentService _employeeArrangmentService;
        public EmployeeArrangmentController(IEmployeeArrangmentService employeearrangmentService, IEmployeeArrangmentService employeeArrangmentService) : base(employeearrangmentService)
        {
            _employeeArrangmentService = employeeArrangmentService;
        }

        [HttpGet("available-guides")]
        public async Task<IActionResult> GetAvailableGuides([FromQuery] string arrangementId, [FromQuery] string dateFrom, [FromQuery] string? dateTo)
        {
            return Ok(await this._employeeArrangmentService.GetAvailableEmployeeArrangmentsAsync(new Guid(arrangementId), dateFrom, dateTo));
        }

    }
}