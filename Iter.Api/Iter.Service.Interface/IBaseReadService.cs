using Iter.Core.Models;

namespace Iter.Services.Interfaces
{
    public interface IBaseReadService<T, TGet, TSearch> where TGet : class
    {
        Task<List<TGet>> GetAll();

        Task<PagedResult<TGet>> Get(TSearch searchObject);

        Task<TGet> GetById(Guid id);
    }
}