using AspNetCore.Reporting;
using Iter.Core.Enum;
using Iter.Core.Search_Models;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;
        public ReportController(IReportService reportService)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            this.reportService = reportService;
        }

        [HttpPost("userPaymentReport")]
        public async Task<IActionResult> PrintUserPaymentReport([FromBody] ReportSearchModel searchModel)
        {
            var data = await this.reportService.UserPaymentReport(searchModel.ArrangementId, searchModel.DateFrom, searchModel.DateFrom);
            return this.Ok(data);
        }

        [HttpPost("arrangementEarnings")]
        public async Task<IActionResult> PrintArrangementEarningsReport([FromBody] ReportSearchModel searchModel)
        {
            var data = await this.reportService.ArrangementEarningsReport(searchModel.AgencyId, searchModel.DateFrom, searchModel.DateFrom);
            return this.Ok(data);
        }
    }
}