using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Core.Models;

namespace Iter.Repository.Interface
{
    public interface IEmployeeArrangmentRepository : IBaseCrudRepository<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>
    {
        Task<List<Employee>> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, DateTime dateFrom, DateTime? dateTo);

        Task AddRangeAsync(List<EmployeeArrangment> employeeArrangments);
    }
}