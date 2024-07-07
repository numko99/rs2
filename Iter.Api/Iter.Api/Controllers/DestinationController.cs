using Iter.API.Controllers;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class DestinationController : BaseCRUDController<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel, DestinationResponse>
    {
        public DestinationController(IDestinationService destinationService) : base(destinationService)
        {
        }
    }
}