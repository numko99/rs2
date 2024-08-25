using Iter.Infrastrucure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class BaseReadRepository<T> : IBaseReadRepository<T> where T : class
    {
        private readonly IterContext dbContext;
        private readonly DbSet<T> dbSet;
        private readonly ILogger<BaseReadRepository<T>> logger;

        public BaseReadRepository(IterContext dbContext, ILogger<BaseReadRepository<T>> logger)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
            this.logger = logger;
        }

        public async virtual Task<T?> GetById(Guid id)
        {
            logger.LogInformation("GetById operation started for ID: {Id}", id);

            try
            {
                var entity = await this.dbSet.FindAsync(id);
                if (entity == null)
                {
                    logger.LogWarning("GetById operation completed: No entity found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);
                }

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for ID: {Id}", id);
                throw;
            }
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            logger.LogInformation("GetAll operation started.");

            try
            {
                var entities = await this.dbSet.ToListAsync();
                logger.LogInformation("GetAll operation completed successfully with {Count} entities retrieved.", entities.Count);

                return entities;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetAll operation.");
                throw;
            }
        }
    }
}
