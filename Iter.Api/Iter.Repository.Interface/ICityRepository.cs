using Iter.Core.EntityModelss;
using Iter.Core.Models;

namespace Iter.Repository.Interface
{
    public interface ICityRepository : IBaseCrudRepository<City>
    {
        Task<City> GetById(int id);

        Task<PagedResult<City>> Get(int? pageSize, int? currentPage, string? name, int? countryId);
    }
}