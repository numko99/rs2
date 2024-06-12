using Iter.API.Controllers;
using Iter.Core;
using Iter.Core.Enum;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : BaseCRUDController<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel, DestinationResponse>
    {
        public DestinationController(IDestinationService destinationService) : base(destinationService)
        {
        }
    }
}