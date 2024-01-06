using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : BaseCRUDController<Reservation, ReservationUpsertRequest, ReservationUpsertRequest, ReservationResponse, AgencySearchModel>
    {
        public ReservationController(IReservationService reservationService) : base(reservationService)
        {
        }
    }
}