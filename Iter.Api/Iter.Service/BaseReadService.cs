using AutoMapper;
using Iter.Core.Models;
using Iter.Core.Search_Models;
using Iter.Repository;
using Iter.Services.Interfaces;

namespace Iter.Services
{
    public class BaseReadService<T, TGet, TSearch> : IBaseReadService<T, TGet, TSearch> where T : class where TGet : class where TSearch: BaseSearchModel
    {
        protected readonly IBaseReadRepository<T, TGet, TSearch> baseReadRepository;
        private readonly IMapper mapper;


        public BaseReadService(IBaseReadRepository<T, TGet, TSearch> baseReadRepository, IMapper mapper)
        {
            this.baseReadRepository = baseReadRepository;
            this.mapper = mapper;

        }

        public virtual async Task<PagedResult<TGet>> Get(TSearch searchObject)
        {
            return await this.baseReadRepository.Get(searchObject);
        }

        public virtual async Task<List<TGet>> GetAll()
        {
            var entities = await this.baseReadRepository.GetAll();
            return this.mapper.Map<List<TGet>>(entities);
        }

        public virtual async Task<TGet> GetById(Guid id)
        {
            var entity = await this.baseReadRepository.GetById(id);
            if (entity == null)
            {
                throw new ArgumentException("Invalid request");
            }
            var a = this.mapper.Map<TGet>(entity);

            return a;
        }
    }
}