using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Model;
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
    public class AgencyController : BaseCRUDController<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel, AgencyResponse>
    {
        public AgencyController(IAgencyService agencyService, ILogger<AgencyController> logger): base(agencyService, logger)
        {
        }
    }
}
