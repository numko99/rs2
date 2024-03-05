using Iter.Core.Enum;

namespace Iter.Services.Interface
{
    public interface IReportPathGetterService
    {
        string GetPath(ReportType reportType);
    }
}
