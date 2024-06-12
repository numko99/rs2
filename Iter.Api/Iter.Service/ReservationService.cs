using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Enum;
using ReservationStatus = Iter.Core.Enum.ReservationStatus;
using Iter.Core.Models;

namespace Iter.Services
{
    public class ReservationService : BaseCrudService<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>, IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IUserAuthenticationService authenticationService;
        private readonly IMapper mapper;
        public ReservationService(IReservationRepository reservationRepository, IMapper mapper, IUserAuthenticationService authenticationService) : base(reservationRepository, mapper)
        {
            this.mapper = mapper;
            this.reservationRepository = reservationRepository;
            this.authenticationService = authenticationService;
        }

        public async Task<int> GetCount(Guid arrangementId)
        {
            var count = await this.reservationRepository.GetCount(arrangementId);

            return count;
        }

        public override async Task<PagedResult<ReservationSearchResponse>> Get(ReservationSearchModel searchObject)
        {
            if (searchObject.AgencyId == null && searchObject.UserId == null)
            {
                var currentUser = await authenticationService.GetCurrentUserAsync();
                if ((Roles)currentUser.Role == Roles.Client)
                {
                    searchObject.UserId = currentUser.Id;
                }
            }

            return await this.baseReadRepository.Get(searchObject);
        }

        public override async Task Insert(ReservationInsertRequest request)
        {
            if (request.ClientId == null) { 
                var currentUser = await this.authenticationService.GetCurrentUserAsync();
                request.ClientId = currentUser.Id;
            }

            var entity = this.mapper.Map<Reservation>(request);
            entity.ReservationStatusId = (int)ReservationStatus.Pending;
            int.TryParse(await this.reservationRepository.GetLatestReservationNumber(), out var latestReservationNumber);
            entity.ReservationNumber = (++latestReservationNumber).ToString();
            entity.TotalPaid = 0;
            await this.reservationRepository.AddAsync(entity);
        }

        public async Task AddReview(Guid reservationId, int? rating)
        {
            await this.reservationRepository.UpdateRatingAsync(reservationId, rating);
        }

        public async Task CancelReservation(Guid reservationId)
        {
            var reservation = await this.reservationRepository.GetById(reservationId);

            if (reservation == null)
            {
                throw new NullReferenceException("No reservation found");
            }

            reservation.ReservationStatusId = (int)ReservationStatus.Cancelled;
            await this.reservationRepository.UpdateAsync(reservation);
        }
    }
}