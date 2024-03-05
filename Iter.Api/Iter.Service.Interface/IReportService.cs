using AspNetCore.Reporting;

namespace Iter.Services.Interface
{
    public interface IReportService
    {
        Task<ReportResult> UserPaymentReport(string arrangementId);

        Task<ReportResult> ArrangementEarningsReport(string agencyId);
    }
}