using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Models;
using Iter.Core.EntityModelss;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class CityRepository : BaseCrudRepository<City>, ICityRepository
    {
        private readonly IterContext dbContext;
        private readonly ILogger<CityRepository> logger;

        public CityRepository(IterContext dbContext, ILogger<CityRepository> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<PagedResult<City>> Get(int? pageSize, int? currentPage, string? name, int? countryId)
        {
            logger.LogInformation("Get operation started with filters: name={Name}, countryId={CountryId}, pageSize={PageSize}, currentPage={CurrentPage}", name, countryId, pageSize, currentPage);

            try
            {
                var query = dbContext.Set<City>().AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(a => a.Name.Contains(name));
                }

                if (countryId != null)
                {
                    query = query.Where(a => a.CountryId == countryId);
                }

                var totalCount = await query.CountAsync();

                query = query.OrderByDescending(q => q.Id);

                if (currentPage.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((currentPage.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                query = query.Include(a => a.Country);

                var resultList = await query.ToListAsync();

                logger.LogInformation("Get operation completed successfully with {Count} results.", resultList.Count);

                return new PagedResult<City>
                {
                    Result = resultList,
                    Count = totalCount
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with filters: name={Name}, countryId={CountryId}, pageSize={PageSize}, currentPage={CurrentPage}", name, countryId, pageSize, currentPage);
                throw;
            }
        }

        public async Task<City> GetById(int id)
        {
            logger.LogInformation("GetById operation started for City ID: {Id}", id);

            try
            {
                var city = await dbContext.City
                    .Include(x => x.Country)
                    .Where(c => c.Id == id)
                    .FirstOrDefaultAsync();

                if (city == null)
                {
                    logger.LogWarning("GetById operation completed: No city found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for City ID: {Id}", id);
                }

                return city;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for City ID: {Id}", id);
                throw;
            }
        }
    }
}
