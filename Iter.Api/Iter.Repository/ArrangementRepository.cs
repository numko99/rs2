using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class ArrangementRepository : BaseCrudRepository<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel>, IArrangementRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public ArrangementRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async override Task<PagedResult<ArrangementResponse>> Get(ArrangmentSearchModel? search)
        {
            var query = dbContext.Set<Arrangement>().AsQueryable();

            PagedResult<ArrangementResponse> result = new PagedResult<ArrangementResponse>();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                query = query.Where(a => a.Name.Contains(search.Name));
            }

            if (!string.IsNullOrEmpty(search?.AgencyId))
            {
                var guid = new Guid(search.AgencyId);
                query = query.Where(a => a.AgencyId == guid);
            }

            if (search?.DateFrom != null)
            {
                query = query.Where(a => a.StartDate >= search.DateFrom);
            }

            if (search?.DateTo != null)
            {
                query = query.Where(a => a.EndDate < search.DateTo);
            }

            query = query.Where(q => q.IsDeleted == false);


            query = query.Include(nameof(Agency));

            result.Count = await query.CountAsync();

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }
            var list = await query.OrderByDescending(q => q.DateCreated).ToListAsync();
            var tmp = mapper.Map<List<ArrangementResponse>>(list);
            result.Result = tmp;
            return result;
        }
    }
}