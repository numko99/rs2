using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IEmployeeArrangmentService : IBaseCrudService<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, AgencySearchModel>
    {
    }
}