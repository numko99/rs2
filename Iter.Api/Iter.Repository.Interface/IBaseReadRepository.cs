using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iter.Repository
{
    public interface IBaseReadRepository<T, TGet> where TGet : class
    {
        Task<T?> GetById(Guid id);

        Task<IEnumerable<T>> GetAll();
    }
}