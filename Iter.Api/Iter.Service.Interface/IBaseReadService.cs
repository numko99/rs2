namespace Iter.Services.Interfaces
{
    public interface IBaseReadService<T, TGet> where TGet : class
    {
        Task<List<TGet>> GetAll();
        Task<TGet> GetById(Guid id);
    }
}