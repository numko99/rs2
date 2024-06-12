using Iter.Core.Models;
using Iter.Core.Search_Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iter.Repository
{
    public interface IBaseReadRepository<T, TGet, TSearchRequest, TSearchResponse> where TGet : class where TSearchResponse : class where TSearchRequest : BaseSearchModel
    {
        Task<T?> GetById(Guid id);

        Task<PagedResult<TSearchResponse>> Get(TSearchRequest? searchObject);

        Task<IEnumerable<T>> GetAll();
    }
}