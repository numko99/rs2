using Iter.Core;
using Iter.Infrastrucure;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class BaseReadRepository<T, TGet> : IBaseReadRepository<T, TGet> where T : class where TGet : class
    {
        private readonly IterContext dbContext;
        private readonly DbSet<T> dbSet;

        public BaseReadRepository(IterContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public async virtual Task<T?> GetById(Guid id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await this.dbSet.ToListAsync();
        }
    }
}