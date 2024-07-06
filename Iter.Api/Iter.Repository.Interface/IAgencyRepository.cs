using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Core.Models;

namespace Iter.Repository.Interface
{
    public interface IAgencyRepository : IBaseCrudRepository<Agency>
    {
        Task<Agency?> GetByEmployeeId(Guid employeeId);

        Task<PagedResult<Agency>> Get(string? name, int? pageSize, int? currentPage);
    }
}
