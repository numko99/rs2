namespace Iter.Services.Interfaces
{
    public interface IBaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> : IBaseReadService<T, TGet, TSearchRequest, TSearchResponse> where TInsert : class where T : class where TGet : class where TSearchResponse : class
    {
        Task Insert(TInsert request);

        Task Update(Guid id, TUpdate request);

        Task Delete(Guid id);
    }
}