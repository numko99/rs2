namespace Iter.Services.Interfaces
{
    public interface IBaseCrudService<T, TInsert, TUpdate, TGet, TSearch> : IBaseReadService<T, TGet, TSearch> where TInsert : class where T : class where TGet : class
    {
        Task Insert(TInsert request);

        Task Update(Guid id, TUpdate request);

        Task Delete(Guid id);
    }
}