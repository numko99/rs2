using Iter.Core.EntityModels;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Iter.Core.Models;

namespace Iter.Repository
{
    public class CityRepository : BaseCrudRepository<City>, ICityRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public CityRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<PagedResult<City>> Get(int? pageSize, int? currentPage, string? name, int? countryId)
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

            return new PagedResult<City>
            {
                Result = resultList,
                Count = totalCount
            };
        }

        public async Task<City> GetById(int id)
        {
            return await this.dbContext.City.Include(x => x.Country).Where(c => c.Id == id).FirstOrDefaultAsync();
        }

    }
}