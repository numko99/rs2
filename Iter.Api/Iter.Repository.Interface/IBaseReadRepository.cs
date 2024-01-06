using Iter.Core.Models;
using Iter.Core.Search_Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iter.Repository
{
    public interface IBaseReadRepository<T, TGet, TSearch> where TGet : class where TSearch : BaseSearchModel
    {
        Task<T?> GetById(Guid id);

        Task<PagedResult<TGet>> Get(TSearch? searchObject);

        Task<IEnumerable<T>> GetAll();
    }
}