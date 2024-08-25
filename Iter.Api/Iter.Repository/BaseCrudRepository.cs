using Iter.Infrastrucure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class BaseCrudRepository<T> : BaseReadRepository<T> where T : class
    {
        private readonly IterContext dbContext;
        private readonly DbSet<T> dbSet;
        private readonly ILogger<BaseCrudRepository<T>> logger;

        public BaseCrudRepository(IterContext dbContext, ILogger<BaseCrudRepository<T>> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
            this.logger = logger;
        }

        public async virtual Task AddAsync(T entity)
        {
            logger.LogInformation("AddAsync operation started for entity of type {EntityType}.", typeof(T).Name);

            try
            {
                await dbSet.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("AddAsync operation completed successfully for entity of type {EntityType}.", typeof(T).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during AddAsync operation for entity of type {EntityType}.", typeof(T).Name);
                throw;
            }
        }

        public async virtual Task UpdateAsync(T entity)
        {
            logger.LogInformation("UpdateAsync operation started for entity of type {EntityType}.", typeof(T).Name);

            try
            {
                this.dbContext.Entry(entity).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();
                logger.LogInformation("UpdateAsync operation completed successfully for entity of type {EntityType}.", typeof(T).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during UpdateAsync operation for entity of type {EntityType}.", typeof(T).Name);
                throw;
            }
        }

        public async virtual Task DeleteAsync(T entity)
        {
            logger.LogInformation("DeleteAsync operation started for entity of type {EntityType}.", typeof(T).Name);

            try
            {
                this.dbSet.Remove(entity);
                await this.dbContext.SaveChangesAsync();
                logger.LogInformation("DeleteAsync operation completed successfully for entity of type {EntityType}.", typeof(T).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during DeleteAsync operation for entity of type {EntityType}.", typeof(T).Name);
                throw;
            }
        }
    }
}
