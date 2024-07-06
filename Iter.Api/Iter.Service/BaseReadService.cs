using AutoMapper;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Repository;
using Iter.Services.Interfaces;

namespace Iter.Services
{
    public class BaseReadService<T, TGet, TSearchRequest, TSearchResponse> : IBaseReadService<T, TGet, TSearchRequest, TSearchResponse> where T : class where TGet : class where TSearchResponse : class where TSearchRequest: BaseSearchModel
    {
        protected readonly IBaseReadRepository<T> baseReadRepository;
        private readonly IMapper mapper;


        public BaseReadService(IBaseReadRepository<T> baseReadRepository, IMapper mapper)
        {
            this.baseReadRepository = baseReadRepository;
            this.mapper = mapper;

        }

        public virtual Task<PagedResult<TSearchResponse>> Get(TSearchRequest searchObject)
        {
            // TODO
            throw new NotImplementedException();
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