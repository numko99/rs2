using AutoMapper;
using Iter.Services.Interface;
using Iter.Repository.Interface;
using Iter.Core.RequestParameterModels;
using Iter.Core.Responses;
using Iter.Core.EntityModels;
using Iter.Core.Enum;

namespace Iter.Services
{
    public class ReportService : IReportService
    {
        private readonly IReservationService reservationService;
        private readonly IReservationRepository reservationRepository;
        private readonly IReportPathGetterService reportPathGetterService;
        private readonly IMapper mapper;
        public ReportService(IReservationService reservationService, IMapper mapper, IReportPathGetterService reportPathGetterService, IReservationRepository reservationRepository)
        {
            this.mapper = mapper;
            this.reservationService = reservationService;
            this.reportPathGetterService = reportPathGetterService;
            this.reservationRepository = reservationRepository;
        }

        public async Task<List<UserPaymentResponse>> UserPaymentReport(string arrangementId, string? dateFrom, string? dateTo)
        {
            var list = await this.reservationRepository.Get(new ReservationSearchRequesParameters(){ 
                                                            ArrangementId = arrangementId,
                                                            DateTo = dateTo,
                                                            DateFrom = dateFrom,
                                                            ReservationStatusId = ((int)Core.Enum.ReservationStatus.Confirmed).ToString()
            });
            return this.mapper.Map<List<UserPaymentResponse>>(list.Result);
        }

        public async Task<List<ArrangementEarnings>> ArrangementEarningsReport(string agencyId, string? dateFrom, string? dateTo)
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

            return groupedList;
        }

    }

}
