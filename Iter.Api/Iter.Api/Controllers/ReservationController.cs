using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core.Requests;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide) + "," + nameof(Roles.Client))]
    public class ReservationController : BaseCRUDController<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>
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

        [HttpPost("addReview")]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequest request)
        {
            await this.reservationService.AddReview(new Guid(request.ReservationId), request.Rating);
            return Ok();
        }

        [HttpGet("cancelReservation/{reservationId}")]
        public async Task<IActionResult> CancelReservation(string reservationId)
        {
            await this.reservationService.CancelReservation(new Guid(reservationId));
            return Ok();
        }
    }
}