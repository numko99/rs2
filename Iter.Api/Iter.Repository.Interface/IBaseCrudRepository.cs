using Iter.Core.Search_Models;

namespace Iter.Repository
{
    public interface IBaseCrudRepository<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> : IBaseReadRepository<T, TGet, TSearchRequest, TSearchResponse> where TInsert : class where T : class where TGet : class where TSearchResponse : class where TSearchRequest : BaseSearchModel
    {
        Task AddAsync(T entity);
        
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}