using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Iter.Model;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide) + "," + nameof(Roles.Client))]
    public class ReservationController : BaseCRUDController<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>
    {
        private readonly IReservationService reservationService;
        private readonly ILogger<ReservationController> logger;

        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger) : base(reservationService, logger)
        {
            this.reservationService = reservationService;
            this.logger = logger;
        }

        [HttpGet("count/{arrangementId}")]
        public async Task<IActionResult> GetCount(string arrangementId)
        {
            logger.LogInformation("GetCount operation started for arrangementId: {ArrangementId}", arrangementId);

            try
            {
                var count = await reservationService.GetCount(new Guid(arrangementId));
                logger.LogInformation("GetCount operation completed successfully for arrangementId: {ArrangementId} with count: {Count}", arrangementId, count);
                return Ok(count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCount operation for arrangementId: {ArrangementId}", arrangementId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("addReview")]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequest request)
        {
            logger.LogInformation("AddReview operation started for ReservationId: {ReservationId}", request.ReservationId);

            try
            {
                await reservationService.AddReview(new Guid(request.ReservationId), request.Rating);
                logger.LogInformation("AddReview operation completed successfully for ReservationId: {ReservationId}", request.ReservationId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during AddReview operation for ReservationId: {ReservationId}", request.ReservationId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("addPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentRequest request)
        {
            logger.LogInformation("AddPayment operation started for ReservationId: {ReservationId}", request.ReservationId);

            try
            {
                await reservationService.AddPayment(new Guid(request.ReservationId!), request.TotalPaid, request.TransactionId);
                logger.LogInformation("AddPayment operation completed successfully for ReservationId: {ReservationId}", request.ReservationId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during AddPayment operation for ReservationId: {ReservationId}", request.ReservationId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("cancelReservation/{reservationId}")]
        public async Task<IActionResult> CancelReservation(string reservationId)
        {
            logger.LogInformation("CancelReservation operation started for ReservationId: {ReservationId}", reservationId);

            try
            {
                await reservationService.CancelReservation(new Guid(reservationId));
                logger.LogInformation("CancelReservation operation completed successfully for ReservationId: {ReservationId}", reservationId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during CancelReservation operation for ReservationId: {ReservationId}", reservationId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public override async Task<IActionResult> Insert([FromBody] ReservationInsertRequest request)
        {
            logger.LogInformation("Insert operation started for new reservation with request data: {@Request}", request);

            try
            {
                var reservation = await reservationService.Insert(request);
                logger.LogInformation("Insert operation completed successfully with new ReservationId: {ReservationId}", reservation.Id);
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Insert operation for new reservation with request data: {@Request}", request);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
