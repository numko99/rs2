using Iter.Core.EntityModels;
using Iter.Core.Models;

namespace Iter.Repository.Interface
{
    public interface ICountryRepository : IBaseCrudRepository<Country>
    {
        Task<Country> GetById(int id);

        Task<PagedResult<Country>> Get(int? pageSize, int? currentPage, string? name);
    }
}