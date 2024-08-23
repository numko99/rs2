using Iter.Core.Responses;

namespace Iter.Services.Interface
{
    public interface IStatisticService
    {
        Task<AdminStatisticResponse?> GetAdminStatistic();

        Task<AdminStatisticResponse?> GetEmployeeStatistic(string agencyId);
    }
}
