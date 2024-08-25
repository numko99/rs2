using Iter.Model;
using Iter.Core.RequestParameterModels;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IAgencyRepository agencyRepository;
        private readonly IReservationRepository reservationRepository;
        private readonly IArrangementRepository arrangementRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<StatisticService> logger;

        public StatisticService(IAgencyRepository agencyRepository, IReservationRepository reservationRepository, IArrangementRepository arrangementRepository, IUserRepository userRepository, IMapper mapper, ILogger<StatisticService> logger)
        {
            this.agencyRepository = agencyRepository;
            this.reservationRepository = reservationRepository;
            this.arrangementRepository = arrangementRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<AdminStatisticResponse?> GetAdminStatistic()
        {
            logger.LogInformation("Fetching admin statistics.");

            try
            {
                var reservations = await reservationRepository.Get(new ReservationSearchRequesParameters());
                logger.LogInformation("Fetched {Count} reservations.", reservations.Result.Count);

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

                logger.LogInformation("Admin statistics calculated successfully.");
                return adminStatisticResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching admin statistics.");
                throw;
            }
        }

        public async Task<AdminStatisticResponse?> GetEmployeeStatistic(string agencyId)
        {
            logger.LogInformation("Fetching employee statistics for agencyId: {AgencyId}", agencyId);

            try
            {
                var reservations = await reservationRepository.Get(new ReservationSearchRequesParameters()
                {
                    AgencyId = agencyId
                });
                logger.LogInformation("Fetched {Count} reservations for agencyId: {AgencyId}", reservations.Result.Count, agencyId);

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

                logger.LogInformation("Employee statistics for agencyId: {AgencyId} calculated successfully.", agencyId);
                return adminStatisticResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching employee statistics for agencyId: {AgencyId}.", agencyId);
                throw;
            }
        }
    }
}
