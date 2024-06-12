using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Iter.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class EmployeeArrangmentRepository : BaseCrudRepository<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>, IEmployeeArrangmentRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public EmployeeArrangmentRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task AddRangeAsync(List<EmployeeArrangment> employeeArrangments)
        {
            var oldEmployeArrangements = await this.dbContext.EmployeeArrangment.Where(x => x.ArrangementId == employeeArrangments.FirstOrDefault().ArrangementId && x.IsDeleted == false).ToListAsync();
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

        public virtual async Task<PagedResult<EmployeeArrangmentResponse>> Get(EmployeeArrangementSearchModel? search = null)
        {
            var query = dbContext.Set<EmployeeArrangment>().AsQueryable();

            PagedResult<EmployeeArrangmentResponse> result = new PagedResult<EmployeeArrangmentResponse>();

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
                if (search.ReturnActiveArrangements == null)
                {
                    query = query.Where(q => q.Arrangement.EndDate != null ? q.Arrangement.EndDate.Value.Date >= DateTime.Now.Date : q.Arrangement.StartDate.Date >= DateTime.Now.Date).AsQueryable();
                    query = query.OrderBy(q => q.Arrangement.StartDate).AsQueryable();
                }
            }

            if (search?.ArrangementId != null)
            {
                query = query.Where(q => q.ArrangementId == search.ArrangementId && q.IsDeleted == false).AsQueryable();
            }

            query = query.Include(a => a.Employee).Include(a => a.Arrangement).ThenInclude(a => a.Agency).Include(a => a.Arrangement).ThenInclude(a => a.ArrangementImages).ThenInclude(a => a.Image);


            result.Count = await query.CountAsync();

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }

            var list = await query.ToListAsync();

            var tmp = mapper.Map<List<EmployeeArrangmentResponse>>(list);
            result.Result = tmp;
            return result;
        }

        public async Task<List<Employee>> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, DateTime dateFrom, DateTime? dateTo)
        {
            var query = await this.dbContext.Employee.Where(e => !e.EmployeeArrangments.Any(x => (x.Arrangement.StartDate.Date <= (dateTo != null ? dateTo.Value.Date : dateFrom.Date) && (x.Arrangement.EndDate != null ? x.Arrangement.EndDate.Value.Date >= dateFrom.Date : x.Arrangement.StartDate.Date >= dateFrom.Date) && x.IsDeleted == false))).ToListAsync();
            return query;
        }
    }
}