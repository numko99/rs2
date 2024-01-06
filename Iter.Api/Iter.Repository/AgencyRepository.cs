using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class AgencyRepository : BaseCrudRepository<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel>, IAgencyRepository
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

            query = query.Include(nameof(Address));

            result.Count = await query.CountAsync();

            query = query.OrderByDescending(q => q.DateCreated);

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }
            var list = await query.ToListAsync();
            var tmp = mapper.Map<List<AgencyResponse>>(list);
            result.Result = tmp;
            return result;
        }

        public async override Task AddAsync(Agency entity)
        {
            var address = entity.Address;
            await this.dbContext.Address.AddAsync(address);
            await dbContext.SaveChangesAsync();

            await this.dbContext.Agency.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async override Task<Agency?> GetById(Guid id)
        {
            return await this.dbContext.Agency.Include(a => a.Address).FirstOrDefaultAsync(a => a.Id == id);
        }

        public override async Task DeleteAsync(Agency entity)
        {
            var address = entity.Address;
            this.dbContext.Agency.Remove(entity);
            this.dbContext.Address.Remove(address);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
