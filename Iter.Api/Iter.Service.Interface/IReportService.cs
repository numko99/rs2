using Iter.Model;

namespace Iter.Services.Interface
{
    public interface IReportService
    {
        Task<List<UserPaymentResponse>> UserPaymentReport(string agencyId, string? dateFrom, string? dateTo);

        Task<List<ArrangementEarnings>> ArrangementEarningsReport(string agencyId, string? dateFrom, string? dateTo);
    }
}