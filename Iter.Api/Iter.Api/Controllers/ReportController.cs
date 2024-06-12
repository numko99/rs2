using AspNetCore.Reporting;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;
        public ReportController(IReportService reportService)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            this.reportService = reportService;
        }

        [HttpPost("userPaymentReport/{arrangementId}")]
        public async Task<IActionResult> PrintUserPaymentReport(string arrangementId)
        {
            ReportResult result = await this.reportService.UserPaymentReport(arrangementId);
            return File(result.MainStream, "application/pdf", "UserPaymentsReport.pdf");
        }

        [HttpPost("arrangementEarnings/{agencyId}")]
        public async Task<IActionResult> PrintArrangementEarningsReport(string agencyId)
        {
            ReportResult result = await this.reportService.ArrangementEarningsReport(agencyId);
            return File(result.MainStream, "application/pdf", "arrangementEarnings.pdf");
        }
    }
}