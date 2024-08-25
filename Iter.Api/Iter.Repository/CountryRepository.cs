using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Models;
using Microsoft.Extensions.Logging;
using Iter.Core.EntityModelss;

namespace Iter.Repository
{
    public class CountryRepository : BaseCrudRepository<Country>, ICountryRepository
    {
        private readonly IterContext dbContext;
        private readonly ILogger<CountryRepository> logger;

        public CountryRepository(IterContext dbContext, ILogger<CountryRepository> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<PagedResult<Country>> Get(int? pageSize, int? currentPage, string? name)
        {
            logger.LogInformation("Get operation started with filters: name={Name}, pageSize={PageSize}, currentPage={CurrentPage}", name, pageSize, currentPage);

            try
            {
                var query = dbContext.Set<Country>().AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(a => a.Name.Contains(name));
                }

                var totalCount = await query.CountAsync();

                query = query.OrderByDescending(q => q.Id);

                if (currentPage.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((currentPage.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var resultList = await query.ToListAsync();

                logger.LogInformation("Get operation completed successfully with {Count} results.", resultList.Count);

                return new PagedResult<Country>
                {
                    Result = resultList,
                    Count = totalCount
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with filters: name={Name}, pageSize={PageSize}, currentPage={CurrentPage}", name, pageSize, currentPage);
                throw;
            }
        }

        public async Task<Country> GetById(int id)
        {
            logger.LogInformation("GetById operation started for Country ID: {Id}", id);

            try
            {
                var country = await dbContext.Country.Where(c => c.Id == id).FirstOrDefaultAsync();

                if (country == null)
                {
                    logger.LogWarning("GetById operation completed: No country found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for Country ID: {Id}", id);
                }

                return country;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for Country ID: {Id}", id);
                throw;
            }
        }
    }
}
