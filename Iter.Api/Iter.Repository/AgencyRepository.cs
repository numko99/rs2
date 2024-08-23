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
    public class AgencyRepository : BaseCrudRepository<Agency>, IAgencyRepository
    {
        private readonly IterContext dbContext;

        public AgencyRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
        }

        public async Task<PagedResult<Agency>> Get(string? name, int? pageSize, int? currentPage)
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

            return new PagedResult<Agency>
            {
                Result = resultList,
                Count = totalCount
            };
        }

        public async override Task<Agency?> GetById(Guid id)
        {
            return await this.dbContext.Agency.Include(a => a.Address).ThenInclude(x => x.City).ThenInclude(x => x.Country).Include(a => a.Image).FirstOrDefaultAsync(a => a.Id == id);
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

        public async Task<int> GetCount()
        {
            return await this.dbContext.Agency.CountAsync();
        }
    }
}
