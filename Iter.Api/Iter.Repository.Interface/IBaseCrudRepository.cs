namespace Iter.Repository
{
    public interface IBaseCrudRepository<T> : IBaseReadRepository<T> where T : class
    {
        Task AddAsync(T entity);
        
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}