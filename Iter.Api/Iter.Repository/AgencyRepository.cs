using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class AgencyRepository : BaseCrudRepository<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel, AgencyResponse>, IAgencyRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public AgencyRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public override async Task<PagedResult<AgencyResponse>> Get(AgencySearchModel? search)
        {
            var query = dbContext.Set<Agency>().AsQueryable();

            PagedResult<AgencyResponse> result = new PagedResult<AgencyResponse>();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                query = query.Where(a => a.Name.Contains(search.Name));
            }

            query = query.Where(a => a.IsDeleted == false);

            query = query.Include(a => a.Address).Include(a => a.Image);

            result.Count = await query.CountAsync();

            query = query.OrderByDescending(q => q.CreatedAt);

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }
            var list = await query.ToListAsync();
            var tmp = mapper.Map<List<AgencyResponse>>(list);
            result.Result = tmp;
            return result;
        }


        public async override Task<Agency?> GetById(Guid id)
        {
            return await this.dbContext.Agency.Include(a => a.Address).Include(a => a.Image).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Agency?> GetByEmployeeId(Guid employeeId)
        {
            return await this.dbContext.Employee.Include(x => x.Agency).Where(e =>e.Id == employeeId).Select(a => a.Agency).FirstOrDefaultAsync();
        }

        public override async Task DeleteAsync(Agency entity)
        {
            entity.IsDeleted = true;
            dbContext.Agency.Update(entity);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
