using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Enum;
using ReservationStatus = Iter.Core.Enum.ReservationStatus;

namespace Iter.Services
{
    public class ReservationService : BaseCrudService<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel>, IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IMapper mapper;
        public ReservationService(IReservationRepository reservationRepository, IMapper mapper) : base(reservationRepository, mapper)
        {
            this.mapper = mapper;
            this.reservationRepository = reservationRepository;
        }

        public async Task<int> GetCount(Guid arrangementId)
        {
            var count = await this.reservationRepository.GetCount(arrangementId);

            return count;
        }

        public override async Task Insert(ReservationInsertRequest request)
        {
            var entity = this.mapper.Map<Reservation>(request);
            entity.ReservationStatusId = (int)ReservationStatus.OnHold;
            int.TryParse(await this.reservationRepository.GetLatestReservationNumber(), out var latestReservationNumber);
            entity.ReservationNumber = (++latestReservationNumber).ToString();
            entity.TotalPaid = 0;
            await this.reservationRepository.AddAsync(entity);
        }
    }
}