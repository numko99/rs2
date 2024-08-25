using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class AgencyRepository : BaseCrudRepository<Agency>, IAgencyRepository
    {
        private readonly IterContext dbContext;
        private readonly ILogger<AgencyRepository> logger;

        public AgencyRepository(IterContext dbContext, ILogger<AgencyRepository> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<PagedResult<Agency>> Get(string? name, int? pageSize, int? currentPage)
        {
            logger.LogInformation("Get operation started with filters: name={Name}, pageSize={PageSize}, currentPage={CurrentPage}", name, pageSize, currentPage);

            try
            {
                var query = dbContext.Set<Agency>().Where(a => !a.IsDeleted);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(a => a.Name.Contains(name));
                }

                var totalCount = await query.CountAsync();

                query = query.OrderByDescending(q => q.CreatedAt);

                if (currentPage.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((currentPage.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                query = query.Include(a => a.Address)
                             .ThenInclude(a => a.City)
                             .ThenInclude(x => x.Country);

                var resultList = await query.ToListAsync();

                logger.LogInformation("Get operation completed successfully with {Count} results.", resultList.Count);

                return new PagedResult<Agency>
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

        public async override Task<Agency?> GetById(Guid id)
        {
            logger.LogInformation("GetById operation started for ID: {Id}", id);

            try
            {
                var agency = await dbContext.Agency
                    .Include(a => a.Address)
                    .ThenInclude(x => x.City)
                    .ThenInclude(x => x.Country)
                    .Include(a => a.Image)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (agency == null)
                {
                    logger.LogWarning("GetById operation completed: No agency found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);
                }

                return agency;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for ID: {Id}", id);
                throw;
            }
        }

        public async Task<Agency?> GetByEmployeeId(Guid employeeId)
        {
            logger.LogInformation("GetByEmployeeId operation started for Employee ID: {EmployeeId}", employeeId);

            try
            {
                var agency = await dbContext.Employee
                    .Include(x => x.Agency)
                    .Where(e => e.Id == employeeId)
                    .Select(a => a.Agency)
                    .FirstOrDefaultAsync();

                if (agency == null)
                {
                    logger.LogWarning("GetByEmployeeId operation completed: No agency found for Employee ID: {EmployeeId}", employeeId);
                }
                else
                {
                    logger.LogInformation("GetByEmployeeId operation completed successfully for Employee ID: {EmployeeId}", employeeId);
                }

                return agency;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetByEmployeeId operation for Employee ID: {EmployeeId}", employeeId);
                throw;
            }
        }

        public override async Task DeleteAsync(Agency entity)
        {
            logger.LogInformation("DeleteAsync operation started for Agency ID: {Id}", entity.Id);

            try
            {
                entity.IsDeleted = true;
                dbContext.Agency.Update(entity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("DeleteAsync operation completed successfully for Agency ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during DeleteAsync operation for Agency ID: {Id}", entity.Id);
                throw;
            }
        }

        public async Task<int> GetCount()
        {
            logger.LogInformation("GetCount operation started.");

            try
            {
                var count = await dbContext.Agency.CountAsync();
                logger.LogInformation("GetCount operation completed successfully with count: {Count}", count);
                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCount operation.");
                throw;
            }
        }
    }
}
