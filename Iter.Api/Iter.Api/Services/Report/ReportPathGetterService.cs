using Iter.Core.Enum;
using Iter.Services.Interface;

namespace Iter.Api.Services.Report
{
    public class ReportPathGetterService : IReportPathGetterService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportPathGetterService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public string GetPath(ReportType reportType)
        {
            string reportPath = string.Empty;
            switch (reportType)
            {   
                case ReportType.UserPayment:

                    reportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\UserPaymentsReport.rdlc";

                    if (!System.IO.File.Exists(reportPath))
                    {
                        throw new Exception("Report not found");
                    }
                    break;
                case ReportType.ArrangementEarnings:

                    reportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\ArrangementEarning.rdlc";

                    if (!System.IO.File.Exists(reportPath))
                    {
                        throw new Exception("Report not found");
                    }
                    break;
                default:
                    break;

            }
            return reportPath;
        }
    }
}
