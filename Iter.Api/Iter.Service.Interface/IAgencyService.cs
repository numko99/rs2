using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IAgencyService : IBaseCrudService<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel, AgencyResponse>
    {
        Task<AgencyResponse?> GetByEmployeeId(Guid employeeId);
    }
}
