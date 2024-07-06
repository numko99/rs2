using Iter.API.Controllers;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DestinationController : BaseCRUDController<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel, DestinationResponse>
    {
        public DestinationController(IDestinationService destinationService) : base(destinationService)
        {
        }
    }
}