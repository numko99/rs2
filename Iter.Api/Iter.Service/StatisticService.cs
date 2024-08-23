using Iter.Core.Models;
using Iter.Core;
using Iter.Core.RequestParameterModels;
using Iter.Core.Responses;
using Iter.Core.Responses.Home;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using AutoMapper;

namespace Iter.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IAgencyRepository agencyRepository;
        private readonly IReservationRepository reservationRepository;
        private readonly IArrangementRepository arrangementRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public StatisticService(IAgencyRepository agencyRepository, IReservationRepository reservationRepository, IArrangementRepository arrangementRepository, IUserRepository userRepository, IMapper mapper)
        {
            this.agencyRepository = agencyRepository;
            this.reservationRepository = reservationRepository;
            this.arrangementRepository = arrangementRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<AdminStatisticResponse?> GetAdminStatistic()
        {
            var reservations = await reservationRepository.Get(new ReservationSearchRequesParameters());
            var adminStatisticResponse = new AdminStatisticResponse()
            {
                UsersCount = await userRepository.GetCount(),
                ReservationCount = await reservationRepository.GetCount(),
                AgenciesCount = await agencyRepository.GetCount(),
                ArrangementCount = await arrangementRepository.GetCount(),
                TotalAmount = await reservationRepository.GetTotalAmount(),
                Reservations = reservations.Result.GroupBy(x => x.ReservationDate.Date).Select(x => new ReservationDiagramResponse()
                {
                    Date = x.Key.Date,
                    ReservationCount = x.Count()
                }).ToList()
            };

            return adminStatisticResponse;
        }

        public async Task<AdminStatisticResponse?> GetEmployeeStatistic(string agencyId)
        {
            var reservations = await reservationRepository.Get(new ReservationSearchRequesParameters()
            {
                AgencyId = agencyId
            });

            var adminStatisticResponse = new AdminStatisticResponse()
            {
                ReservationCount = await reservationRepository.GetCount(agencyId: new Guid(agencyId)),
                ArrangementCount = await arrangementRepository.GetCount(new Guid(agencyId)),
                TotalAmount = await reservationRepository.GetTotalAmount(new Guid(agencyId)),
                Reservations = reservations.Result.GroupBy(x => x.ReservationDate.Date).Select(x => new ReservationDiagramResponse()
                {
                    Date = x.Key.Date,
                    ReservationCount = x.Count()
                }).ToList()
            };

            return adminStatisticResponse;
        }
    }
}
