using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Repository
{
    public class AgencyRepository: BaseCrudRepository<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse>, IAgencyRepository
    {
        private readonly IterContext dbContext;
        public AgencyRepository(IterContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<AgencySearchResponse>> GetAgenciesSearch(int currentPage, int pageSize)
        {
            var skip = (currentPage - 1) * pageSize;

            var query = dbContext.Agency.
                        AsNoTracking().
                        OrderByDescending(a => a.DateCreated);

            var totalCount = await query.CountAsync();

            var agencySearchResponses = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(a => new AgencySearchResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    City = a.Address != null ? a.Address.City : null,
                    ContactEmail = a.ContactEmail,
                    ContactPhone = a.ContactPhone,
                    TotalCount = totalCount
                })
                .ToListAsync();

            return agencySearchResponses;
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
