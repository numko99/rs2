using AutoMapper;
using Iter.Services.Interface;
using AspNetCore.Reporting;
using Iter.Core.ReportDatasetModels;
using Iter.Core.Enum;

namespace Iter.Services
{
    public class ReportService : IReportService
    {
        private readonly IReservationService reservationService;
        private readonly IReportPathGetterService reportPathGetterService;
        private readonly IMapper mapper;
        public ReportService(IReservationService reservationService, IMapper mapper, IReportPathGetterService reportPathGetterService)
        {
            this.mapper = mapper;
            this.reservationService = reservationService;
            this.reportPathGetterService = reportPathGetterService;
        }

        public async Task<ReportResult> UserPaymentReport(string arrangementId)
        {
            var reportPath = this.reportPathGetterService.GetPath(ReportType.UserPayment);

            var localReport = new LocalReport(reportPath);

            var list = await this.reservationService.Get(new Core.Search_Models.ReservationSearchModel() { ArrangementId = arrangementId });
            var parameters = new Dictionary<string, string>
            {
                { "agencyName", list.Result.FirstOrDefault()?.AgencyName },
                { "arrangementNameParameter", list.Result.FirstOrDefault()?.ArrangementName },
                { "currentDateParameter", DateTime.UtcNow.ToString("dd.MM.yyyy") }

            };

            localReport.AddDataSource("DataSet1", this.mapper.Map<List<UserPaymentModel>>(list.Result));
            var result = localReport.Execute(RenderType.Pdf, parameters: parameters);
            return result;
        }

        public async Task<ReportResult> ArrangementEarningsReport(string agencyId)
        {
            var reportPath = this.reportPathGetterService.GetPath(ReportType.ArrangementEarnings);

            var localReport = new LocalReport(reportPath);

            var list = await this.reservationService.Get(new Core.Search_Models.ReservationSearchModel() { AgencyId = agencyId });
            var parameters = new Dictionary<string, string>
            {
                { "agencyName", list.Result.FirstOrDefault()?.AgencyName },
                { "currentDateParameter", DateTime.UtcNow.ToString("dd.MM.yyyy") }

            };

            localReport.AddDataSource("DataSet1", this.mapper.Map<List<UserPaymentModel>>(list.Result));
            var result = localReport.Execute(RenderType.Pdf, parameters: parameters);
            return result;
        }

    }

}
