using AutoMapper;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;

using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class BaseCrudRepository<T> : BaseReadRepository<T> where T : class
    {
        private readonly IterContext dbContext;
        private readonly DbSet<T> dbSet;

        public BaseCrudRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public async virtual Task AddAsync(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
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