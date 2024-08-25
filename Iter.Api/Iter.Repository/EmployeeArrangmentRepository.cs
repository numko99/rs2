using Iter.Core.EntityModels;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Iter.Core.Models;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Enum;
using Iter.Core.RequestParameterModels;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class EmployeeArrangmentRepository : BaseCrudRepository<EmployeeArrangment>, IEmployeeArrangmentRepository
    {
        private readonly IterContext dbContext;
        private readonly ILogger<EmployeeArrangmentRepository> logger;

        public EmployeeArrangmentRepository(IterContext dbContext,  ILogger<EmployeeArrangmentRepository> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task AddRangeAsync(List<EmployeeArrangment> employeeArrangments, Guid arrangementId)
        {
            logger.LogInformation("AddRangeAsync operation started for Arrangement ID: {ArrangementId} with {Count} EmployeeArrangments.", arrangementId, employeeArrangments.Count);

            try
            {
                var oldEmployeArrangements = await dbContext.EmployeeArrangment.Where(x => x.ArrangementId == arrangementId && x.IsDeleted == false).ToListAsync();
                var newEmployeIds = employeeArrangments.Select(x => x.EmployeeId).ToList();

                foreach (var item in oldEmployeArrangements)
                {
                    if (!newEmployeIds.Contains(item.EmployeeId))
                    {
                        item.IsDeleted = true;
                    }
                }

                employeeArrangments = employeeArrangments.Where(x => !oldEmployeArrangements.Select(x => x.EmployeeId).Contains(x.EmployeeId)).ToList();
                await dbContext.EmployeeArrangment.AddRangeAsync(employeeArrangments);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("AddRangeAsync operation completed successfully for Arrangement ID: {ArrangementId}.", arrangementId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during AddRangeAsync operation for Arrangement ID: {ArrangementId}.", arrangementId);
                throw;
            }
        }

        public async Task<PagedResult<EmployeeArrangment>> Get(EmployeeArrangementRequestParameters? search = null)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchParameters}", search);

            try
            {
                var query = dbContext.Set<EmployeeArrangment>().AsQueryable();
                var result = new PagedResult<EmployeeArrangment>();

                if (search?.EmployeeId != null)
                {
                    query = query.Where(q => q.EmployeeId == search.EmployeeId && q.IsDeleted == false);

                    if (search.ReturnActiveArrangements == true)
                    {
                        query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date >= DateTime.Now.Date && q.Arrangement.StartDate.Date <= DateTime.Now.Date : q.Arrangement.StartDate.Date == DateTime.Now.Date);
                    }
                    if (search.ReturnActiveArrangements == false)
                    {
                        query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date < DateTime.Now.Date : q.Arrangement.StartDate.Date < DateTime.Now.Date);
                        query = query.OrderByDescending(q => q.Arrangement.StartDate);
                    }
                    if (search.ReturnActiveArrangements == null && search.ReturnAll == null)
                    {
                        query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date >= DateTime.Now.Date : q.Arrangement.StartDate.Date >= DateTime.Now.Date);
                        query = query.OrderBy(q => q.Arrangement.StartDate);
                    }

                    if (search.ReturnAll == true)
                    {
                        query = query.OrderByDescending(q => q.Arrangement.StartDate);
                    }

                    if (search.Name != null)
                    {
                        query = query.Where(q => q.Arrangement.Name.Contains(search.Name));
                    }
                }

                if (search?.ArrangementId != null)
                {
                    query = query.Where(q => q.ArrangementId == search.ArrangementId && q.IsDeleted == false);
                }

                result.Count = await query.CountAsync();

                query = query.Include(a => a.Employee)
                             .ThenInclude(a => a.User)
                             .Include(a => a.Arrangement)
                             .ThenInclude(a => a.Agency)
                             .Include(a => a.Arrangement)
                             .ThenInclude(a => a.ArrangementImages)
                             .ThenInclude(a => a.Image);

                if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
                {
                    query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
                }

                var list = await query.ToListAsync();
                result.Result = list;

                logger.LogInformation("Get operation completed successfully with {Count} results.", list.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchParameters}", search);
                throw;
            }
        }

        public async Task<List<Employee>?> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, DateTime dateFrom, DateTime? dateTo)
        {
            logger.LogInformation("GetAvailableEmployeeArrangmentsAsync operation started for Arrangement ID: {ArrangementId}, DateFrom: {DateFrom}, DateTo: {DateTo}", arrangementId, dateFrom, dateTo);

            try
            {
                var arrangement = await dbContext.Arrangement.FindAsync(arrangementId);

                if (arrangement == null)
                {
                    logger.LogWarning("No arrangement found for ID: {ArrangementId} during GetAvailableEmployeeArrangmentsAsync operation.", arrangementId);
                    return null;
                }

                var query = await dbContext.Employee.Where(e => e.User.Role == (int)Roles.TouristGuide && e.AgencyId == arrangement.AgencyId && !e.EmployeeArrangments.Any(x => (x.Arrangement.StartDate.Date <= (dateTo != null ? dateTo.Value.Date : dateFrom.Date) && (x.Arrangement.EndDate != null ? x.Arrangement.EndDate.Value.Date >= dateFrom.Date : x.Arrangement.StartDate.Date >= dateFrom.Date) && x.IsDeleted == false))).AsNoTracking().ToListAsync();

                logger.LogInformation("GetAvailableEmployeeArrangmentsAsync operation completed successfully with {Count} available employees.", query.Count);

                return query;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetAvailableEmployeeArrangmentsAsync operation for Arrangement ID: {ArrangementId}, DateFrom: {DateFrom}, DateTo: {DateTo}", arrangementId, dateFrom, dateTo);
                throw;
            }
        }
    }
}
