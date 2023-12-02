namespace Iter.Services.Interfaces
{
    public interface IBaseCrudService<T, TInsert, TUpdate, TGet> : IBaseReadService<T, TGet> where TInsert : class where T : class where TGet : class
    {
        Task Insert(TInsert request);
        Task Update(Guid id, TUpdate request);
        Task Delete(Guid id);
    }
}