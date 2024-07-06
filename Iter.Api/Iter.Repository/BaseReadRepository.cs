using AutoMapper;
using Iter.Core;
using Iter.Core.Models;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class BaseReadRepository<T> : IBaseReadRepository<T> where T : class
    {
        private readonly IterContext dbContext;
        private readonly DbSet<T> dbSet;
        private readonly IMapper mapper;

        public BaseReadRepository(IterContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
            this.mapper = mapper;
        }

        public async virtual Task<T?> GetById(Guid id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await this.dbSet.ToListAsync();
        }

        //public virtual async Task<PagedResult<TSearchResponse>> Get(TSearchRequest? search = null)
        //{
        //    var query = dbContext.Set<T>().AsQueryable();

        //    PagedResult<TSearchResponse> result = new PagedResult<TSearchResponse>();

        //    query = AddFilter(query, search);

        //    query = AddInclude(query, search);

        //    result.Count = await query.CountAsync();

        //    if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
        //    {
        //        query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
        //    }

        //    var list = await query.ToListAsync();

        //    var tmp = mapper.Map<List<TSearchResponse>>(list);
        //    result.Result = tmp;
        //    return result;
        //}
    }
}