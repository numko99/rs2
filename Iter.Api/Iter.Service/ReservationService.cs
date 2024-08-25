using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Enum;
using ReservationStatus = Iter.Core.Enum.ReservationStatus;
using Iter.Core.Models;
using Iter.Core.Helper;
using Iter.Core.RequestParameterModels;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class ReservationService : BaseCrudService<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>, IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IUserAuthenticationService authenticationService;
        private readonly IMapper mapper;
        private readonly ILogger<ReservationService> logger;

        public ReservationService(IReservationRepository reservationRepository, IMapper mapper, IUserAuthenticationService authenticationService, ILogger<ReservationService> logger)
            : base(reservationRepository, mapper, logger)
        {
            this.mapper = mapper;
            this.reservationRepository = reservationRepository;
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        public async Task<int> GetCount(Guid arrangementId)
        {
            logger.LogInformation("Getting count for arrangementId: {ArrangementId}", arrangementId);
            var count = await this.reservationRepository.GetCount(arrangementId);
            logger.LogInformation("Found {Count} reservations for arrangementId: {ArrangementId}", count, arrangementId);
            return count;
        }

        public override async Task<PagedResult<ReservationSearchResponse>> Get(ReservationSearchModel searchObject)
        {
            logger.LogInformation("Starting Get operation with searchObject: {SearchObject}", searchObject);
            if (searchObject.AgencyId == null && searchObject.UserId == null)
            {
                var currentUser = await authenticationService.GetCurrentUserAsync();
                if ((Roles)currentUser.Role == Roles.Client)
                {
                    searchObject.UserId = currentUser.Id;
                }
                logger.LogInformation("Set UserId in searchObject to current user: {UserId}", searchObject.UserId);
            }

            var data = await this.reservationRepository.Get(this.mapper.Map<ReservationSearchRequesParameters>(searchObject));
            logger.LogInformation("Retrieved {Count} reservations", data.Result.Count);

            return this.mapper.Map<PagedResult<ReservationSearchResponse>>(data);
        }

        public async override Task<ReservationResponse> Insert(ReservationInsertRequest request)
        {
            logger.LogInformation("Inserting new reservation with request: {Request}", request);
            if (request.ClientId == null)
            {
                var currentUser = await this.authenticationService.GetCurrentUserAsync();
                request.ClientId = currentUser.Id;
                logger.LogInformation("Set ClientId in request to current user: {ClientId}", request.ClientId);
            }

            var entity = this.mapper.Map<Reservation>(request);
            entity.ReservationStatusId = (int)ReservationStatus.Pending;
            entity.ReservationNumber = RandomGeneratorHelper.GenerateReservationNumber();
            entity.TotalPaid = 0;

            await this.reservationRepository.AddAsync(entity);
            logger.LogInformation("Successfully inserted reservation with ReservationNumber: {ReservationNumber}", entity.ReservationNumber);

            return this.mapper.Map<ReservationResponse>(entity);
        }

        public async Task AddReview(Guid reservationId, int? rating)
        {
            logger.LogInformation("Adding review for reservationId: {ReservationId} with rating: {Rating}", reservationId, rating);
            await this.reservationRepository.UpdateRatingAsync(reservationId, rating);
            logger.LogInformation("Successfully added review for reservationId: {ReservationId}", reservationId);
        }

        public async Task CancelReservation(Guid reservationId)
        {
            logger.LogInformation("Cancelling reservation with reservationId: {ReservationId}", reservationId);
            var reservation = await this.reservationRepository.GetById(reservationId);

            if (reservation == null)
            {
                logger.LogWarning("No reservation found with reservationId: {ReservationId}", reservationId);
                throw new NullReferenceException("No reservation found");
            }

            reservation.ReservationStatusId = (int)ReservationStatus.Cancelled;
            await this.reservationRepository.UpdateAsync(reservation);
            logger.LogInformation("Successfully cancelled reservation with reservationId: {ReservationId}", reservationId);
        }

        public async Task AddPayment(Guid reservationId, int totalPaid, string transactionId)
        {
            logger.LogInformation("Adding payment for reservationId: {ReservationId}, totalPaid: {TotalPaid}, transactionId: {TransactionId}", reservationId, totalPaid, transactionId);
            var reservation = await this.reservationRepository.GetById(reservationId);

            if (reservation == null)
            {
                logger.LogWarning("No reservation found with reservationId: {ReservationId}", reservationId);
                throw new NullReferenceException("No reservation found");
            }

            reservation.ReservationStatusId = (int)ReservationStatus.Confirmed;
            reservation.TotalPaid = totalPaid;
            reservation.TransactionId = transactionId;

            await this.reservationRepository.UpdateAsync(reservation);
            logger.LogInformation("Successfully added payment for reservationId: {ReservationId}", reservationId);
        }
    }
}
