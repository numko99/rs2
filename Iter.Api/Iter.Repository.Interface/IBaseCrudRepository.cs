using Iter.Core.Search_Models;

namespace Iter.Repository
{
    public interface IBaseCrudRepository<T, TInsert, TUpdate, TGet, TSearch> : IBaseReadRepository<T, TGet, TSearch> where TInsert : class where T : class where TGet : class where TSearch : BaseSearchModel
    {
        Task AddAsync(T entity);
        
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}