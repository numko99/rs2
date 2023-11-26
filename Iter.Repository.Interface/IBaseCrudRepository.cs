namespace Iter.Repository
{
    public interface IBaseCrudRepository<T, TInsert, TUpdate, TGet> : IBaseReadRepository<T, TGet> where TInsert : class where T : class where TGet : class
    {
        Task AddAsync(T entity);
        
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}