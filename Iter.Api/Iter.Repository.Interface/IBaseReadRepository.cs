using Iter.Core.Models;
using Iter.Core.Search_Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iter.Repository
{
    public interface IBaseReadRepository<T> where T : class
    {
        Task<T?> GetById(Guid id);

        Task<IEnumerable<T>> GetAll();
    }
}