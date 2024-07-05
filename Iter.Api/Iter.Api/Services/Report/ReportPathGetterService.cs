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

                    reportPath = GeneratePathForReport("UserPaymentsReport.rdlc");

                    if (!System.IO.File.Exists(reportPath))
                    {
                        throw new Exception("Report not found");
                    }
                    break;
                case ReportType.ArrangementEarnings:

                    reportPath = GeneratePathForReport("ArrangementEarning.rdlc");

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

        public string GeneratePathForReport(string path)
        {
            // Ensure the web root path is set
            if (string.IsNullOrWhiteSpace(_webHostEnvironment.WebRootPath))
            {
                _webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // Use Path.DirectorySeparatorChar to ensure cross-platform compatibility
            string reportsFolder = "Reports" + Path.DirectorySeparatorChar;

#if DEBUG
            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Iter.RdlcDesign", reportsFolder + path);
            return reportPath.Replace("Iter.Api" + Path.DirectorySeparatorChar + "Iter.Api", "Iter.Api");
#else
    string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, reportsFolder, path);
    Console.WriteLine(reportPath);
    return reportPath;
#endif
        }
    }
}
