using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccommodationController : BaseCRUDController<Accommodation, AccommodationUpsertRequest, AccommodationUpsertRequest, AccommodationResponse, AgencySearchModel, AccommodationResponse>
    {
        public AccommodationController(IAccommodationService accommodationService) : base(accommodationService)
        {
        }
    }
}