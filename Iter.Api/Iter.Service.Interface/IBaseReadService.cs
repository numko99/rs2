using Iter.Core.Models;

namespace Iter.Services.Interfaces
{
    public interface IBaseReadService<T, TGet, TSearchRequest, TSearchResponse> where TGet : class where TSearchResponse : class
    {
        Task<List<TGet>> GetAll();

        Task<PagedResult<TSearchResponse>> Get(TSearchRequest searchObject);

        Task<TGet> GetById(Guid id);
    }
}