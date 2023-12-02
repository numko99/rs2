using Iter.API.Controllers;
using Iter.Core;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.User))]
    public class AgencyController : BaseCRUDController<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse>
    {
        private readonly IAgencyService agencyService;
        public AgencyController(IAgencyService agencyService): base(agencyService)
        {
            this.agencyService = agencyService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(int currentPage, int pageSize)
        {
            var agencies = await agencyService.GetAgenciesSearch(currentPage, pageSize);

            return Ok(agencies);
        }

        [HttpPost("insert")]
        public override async Task<IActionResult> Insert([FromBody] AgencyInsertRequest request)
        {
            await agencyService.Insert(request);
            return Ok();
        }

        [HttpGet("details")]
        public override async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await this.agencyService.GetById(id));
        }

        [HttpDelete("delete")]
        public override async Task<IActionResult> Delete(Guid id)
        {
            await this.agencyService.Delete(id);
            return Ok();
        }
    }
}
