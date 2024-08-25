using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Microsoft.AspNetCore.Authorization;
using Iter.Core.Enum;
using Iter.Core;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class AccommodationController : BaseCRUDController<Accommodation, AccommodationUpsertRequest, AccommodationUpsertRequest, AccommodationResponse, BaseSearchModel, AccommodationResponse>
    {
        public AccommodationController(IAccommodationService accommodationService, ILogger<AccommodationController> logger) : base(accommodationService, logger)
        {
        }
    }
}