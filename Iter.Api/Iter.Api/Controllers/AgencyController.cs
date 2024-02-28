using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = nameof(Roles.User))]
    public class AgencyController : BaseCRUDController<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel>
    {
        public AgencyController(IAgencyService agencyService): base(agencyService)
        {
        }
    }
}
