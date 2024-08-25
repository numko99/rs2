using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;

namespace Iter.Repository.Interface
{
    public interface IEmployeeArrangmentRepository : IBaseCrudRepository<EmployeeArrangment>
    {
        Task<List<Employee>?> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, DateTime dateFrom, DateTime? dateTo);

        Task AddRangeAsync(List<EmployeeArrangment> employeeArrangments, Guid arrangementId);

        Task<PagedResult<EmployeeArrangment>> Get(EmployeeArrangementRequestParameters? search = null);
    }
}