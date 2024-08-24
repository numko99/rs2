using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iter.Core.Models;
using AutoMapper;

namespace Iter.Repository
{
    public class CountryRepository : BaseCrudRepository<Country>, ICountryRepository
    {
        private readonly IterContext dbContext;
        public CountryRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
        }

        public async Task<PagedResult<Country>> Get(int? pageSize, int? currentPage, string? name)
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

            return new PagedResult<Country>
            {
                Result = resultList,
                Count = totalCount
            };
        }

        public async Task<Country> GetById(int id)
        {
            return await this.dbContext.Country.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}