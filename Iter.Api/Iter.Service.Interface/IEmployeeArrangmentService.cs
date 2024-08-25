using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;
using Iter.Core.Models;

namespace Iter.Services.Interface
{
    public interface IEmployeeArrangmentService : IBaseCrudService<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>
    {
        Task<List<DropdownModel>> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, string dateFrom, string? dateTo);
    }
}