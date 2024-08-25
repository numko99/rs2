using Iter.Core.Responses;
using Iter.Model;

namespace Iter.Services.Interface
{
    public interface IStatisticService
    {
        Task<AdminStatisticResponse?> GetAdminStatistic();

        Task<AdminStatisticResponse?> GetEmployeeStatistic(string agencyId);
    }
}
