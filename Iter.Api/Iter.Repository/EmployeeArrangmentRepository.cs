using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Iter.Core.Models;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Enum;
using Iter.Core.RequestParameterModels;

namespace Iter.Repository
{
    public class EmployeeArrangmentRepository : BaseCrudRepository<EmployeeArrangment>, IEmployeeArrangmentRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public EmployeeArrangmentRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task AddRangeAsync(List<EmployeeArrangment> employeeArrangments, Guid arrangementId)
        {
            var oldEmployeArrangements = await this.dbContext.EmployeeArrangment.Where(x => x.ArrangementId ==  arrangementId && x.IsDeleted == false).ToListAsync();
            var newEmployeIds = employeeArrangments.Select(x => x.EmployeeId).ToList();
            foreach (var item in oldEmployeArrangements)
            {
                 if (!newEmployeIds.Contains(item.EmployeeId))
                {
                    item.IsDeleted = true;
                }
            }

            employeeArrangments = employeeArrangments.Where(x => !oldEmployeArrangements.Select(x => x.EmployeeId).Contains(x.EmployeeId)).ToList();
            await this.dbContext.EmployeeArrangment.AddRangeAsync(employeeArrangments);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<EmployeeArrangment>> Get(EmployeeArrangementRequestParameters? search = null)
        {
            var query = dbContext.Set<EmployeeArrangment>().AsQueryable();

            PagedResult<EmployeeArrangment> result = new PagedResult<EmployeeArrangment>();

            if (search?.EmployeeId != null)
            {
                query = query.Where(q => q.EmployeeId == search.EmployeeId && q.IsDeleted == false).AsQueryable();

                if (search.ReturnActiveArrangements == true)
                {
                    query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date >= DateTime.Now.Date &&  q.Arrangement.StartDate.Date <= DateTime.Now.Date : q.Arrangement.StartDate.Date == DateTime.Now.Date).AsQueryable();
                }
                if (search.ReturnActiveArrangements == false)
                {
                    query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date < DateTime.Now.Date : q.Arrangement.StartDate.Date < DateTime.Now.Date).AsQueryable();
                    query = query.OrderByDescending(q => q.Arrangement.StartDate).AsQueryable();
                }
                if (search.ReturnActiveArrangements == null && search.ReturnAll == null)
                {
                    query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date >= DateTime.Now.Date : q.Arrangement.StartDate.Date >= DateTime.Now.Date).AsQueryable();
                    query = query.OrderBy(q => q.Arrangement.StartDate).AsQueryable();
                }

                if (search.ReturnAll == true)
                {
                    query = query.OrderByDescending(q => q.Arrangement.StartDate).AsQueryable();
                }

                if (search.Name != null)
                {
                    query = query.Where(q => q.Arrangement.Name.Contains(search.Name)).AsQueryable();
                }
            }

            if (search?.ArrangementId != null)
            {
                query = query.Where(q => q.ArrangementId == search.ArrangementId && q.IsDeleted == false).AsQueryable();
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
            return result;
        }

        public async Task<List<Employee>?> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, DateTime dateFrom, DateTime? dateTo)
        {
            var arrangement = await this.dbContext.Arrangement.FindAsync(arrangementId);

            if (arrangement == null)
            {
                return null;
            }

            var query = await this.dbContext.Employee.Where(e => e.User.Role == (int)Roles.TouristGuide && e.AgencyId == arrangement.AgencyId && !e.EmployeeArrangments.Any(x => (x.Arrangement.StartDate.Date <= (dateTo != null ? dateTo.Value.Date : dateFrom.Date) && (x.Arrangement.EndDate != null ? x.Arrangement.EndDate.Value.Date >= dateFrom.Date : x.Arrangement.StartDate.Date >= dateFrom.Date) && x.IsDeleted == false))).AsNoTracking().ToListAsync();
            return query;
        }
    }
}