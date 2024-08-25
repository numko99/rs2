using AspNetCore.Reporting;
using Iter.Core.Enum;
using Iter.Core.Search_Models;
using Iter.Model;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;
        private readonly ILogger<ReportController> logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            this.reportService = reportService;
            this.logger = logger;
        }

        [HttpPost("userPaymentReport")]
        public async Task<IActionResult> PrintUserPaymentReport([FromBody] ReportSearchModel searchModel)
        {
            logger.LogInformation("PrintUserPaymentReport operation started with parameters: ArrangementId={ArrangementId}, DateFrom={DateFrom}, DateTo={DateTo}", searchModel.ArrangementId, searchModel.DateFrom, searchModel.DateTo);

            try
            {
                var data = await reportService.UserPaymentReport(searchModel.ArrangementId, searchModel.DateFrom, searchModel.DateTo);
                logger.LogInformation("PrintUserPaymentReport operation completed successfully.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during PrintUserPaymentReport operation with parameters: ArrangementId={ArrangementId}, DateFrom={DateFrom}, DateTo={DateTo}", searchModel.ArrangementId, searchModel.DateFrom, searchModel.DateTo);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("arrangementEarnings")]
        public async Task<IActionResult> PrintArrangementEarningsReport([FromBody] ReportSearchModel searchModel)
        {
            logger.LogInformation("PrintArrangementEarningsReport operation started with parameters: AgencyId={AgencyId}, DateFrom={DateFrom}, DateTo={DateTo}", searchModel.AgencyId, searchModel.DateFrom, searchModel.DateTo);

            try
            {
                var data = await reportService.ArrangementEarningsReport(searchModel.AgencyId, searchModel.DateFrom, searchModel.DateTo);
                logger.LogInformation("PrintArrangementEarningsReport operation completed successfully.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during PrintArrangementEarningsReport operation with parameters: AgencyId={AgencyId}, DateFrom={DateFrom}, DateTo={DateTo}", searchModel.AgencyId, searchModel.DateFrom, searchModel.DateTo);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
