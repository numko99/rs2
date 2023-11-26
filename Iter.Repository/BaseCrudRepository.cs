using Iter.Infrastrucure;

using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class BaseCrudRepository<T, TInsert, TUpdate, TGet> : BaseReadRepository<T, TGet> where TInsert : class where T : class where TGet : class
    {
        private readonly IterContext dbContext;
        private readonly DbSet<T> dbSet;

        public BaseCrudRepository(IterContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public async virtual Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async virtual Task UpdateAsync(T entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(T entity)
        {
            this.dbSet.Remove(entity);
            await this.dbContext.SaveChangesAsync();
        }
    }
}