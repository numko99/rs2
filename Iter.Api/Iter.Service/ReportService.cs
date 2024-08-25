using AutoMapper;
using Iter.Services.Interface;
using Iter.Repository.Interface;
using Iter.Core.RequestParameterModels;
using Iter.Core.Responses;
using Microsoft.Extensions.Logging;
using Iter.Model;

namespace Iter.Services
{
    public class ReportService : IReportService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ReportService> logger;

        public ReportService(IMapper mapper, IReservationRepository reservationRepository, ILogger<ReportService> logger)
        {
            this.mapper = mapper;
            this.reservationRepository = reservationRepository;
            this.logger = logger;
        }

        public async Task<List<UserPaymentResponse>> UserPaymentReport(string arrangementId, string? dateFrom, string? dateTo)
        {
            logger.LogInformation("Starting UserPaymentReport generation for ArrangementId: {ArrangementId}, DateFrom: {DateFrom}, DateTo: {DateTo}", arrangementId, dateFrom, dateTo);
            try
            {
                var list = await this.reservationRepository.Get(new ReservationSearchRequesParameters()
                {
                    ArrangementId = arrangementId,
                    DateTo = dateTo,
                    DateFrom = dateFrom,
                    ReservationStatusId = ((int)Core.Enum.ReservationStatus.Confirmed).ToString()
                });

                logger.LogInformation("Successfully retrieved {Count} reservations for UserPaymentReport.", list.Result.Count);

                return this.mapper.Map<List<UserPaymentResponse>>(list.Result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while generating UserPaymentReport for ArrangementId: {ArrangementId}", arrangementId);
                throw;
            }
        }

        public async Task<List<ArrangementEarnings>> ArrangementEarningsReport(string agencyId, string? dateFrom, string? dateTo)
        {
            logger.LogInformation("Starting ArrangementEarningsReport generation for AgencyId: {AgencyId}, DateFrom: {DateFrom}, DateTo: {DateTo}", agencyId, dateFrom, dateTo);
            try
            {
                var list = await this.reservationRepository.Get(new ReservationSearchRequesParameters()
                {
                    AgencyId = agencyId,
                    DateTo = dateTo,
                    DateFrom = dateFrom,
                    ReservationStatusId = ((int)Core.Enum.ReservationStatus.Confirmed).ToString()
                });

                var groupedList = list.Result.GroupBy(x => x.ArrangementId).Select(x => new ArrangementEarnings
                {
                    TotalPaid = x.Sum(s => s.TotalPaid),
                    ReservationCount = x.Count(),
                    ArrangementName = x.FirstOrDefault().ArrangementName,
                }).ToList();

                logger.LogInformation("Successfully generated ArrangementEarningsReport with {Count} items for AgencyId: {AgencyId}", groupedList.Count, agencyId);

                return groupedList;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while generating ArrangementEarningsReport for AgencyId: {AgencyId}", agencyId);
                throw;
            }
        }
    }
}
