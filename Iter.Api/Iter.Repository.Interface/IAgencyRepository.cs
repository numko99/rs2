using Iter.Core.EntityModels;
using Iter.Core.Models;

namespace Iter.Repository.Interface
{
    public interface IAgencyRepository : IBaseCrudRepository<Agency>
    {
        Task<Agency?> GetByEmployeeId(Guid employeeId);

        Task<PagedResult<Agency>> Get(string? name, int? pageSize, int? currentPage);

        Task<int> GetCount();
    }
}
