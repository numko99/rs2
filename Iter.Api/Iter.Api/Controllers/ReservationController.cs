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
    public class ReservationController : BaseCRUDController<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel>
    {
        private readonly IReservationService reservationService;
        public ReservationController(IReservationService reservationService) : base(reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpGet("count/{arrangementId}")]
        public async Task<IActionResult> GetCount(string arrangementId)
        {
            return Ok(await this.reservationService.GetCount(new Guid(arrangementId)));
        }
    }
}