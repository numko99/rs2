using AutoMapper;
using Iter.Repository;
using Iter.Services.Interfaces;

namespace Iter.Services
{
    public class BaseReadService<T, TGet> : IBaseReadService<T, TGet> where T : class where TGet : class
    {
        protected readonly IBaseReadRepository<T, TGet> baseReadRepository;
        private readonly IMapper mapper;


        public BaseReadService(IBaseReadRepository<T, TGet> baseReadRepository, IMapper mapper)
        {
            this.baseReadRepository = baseReadRepository;
            this.mapper = mapper;

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
            return this.mapper.Map<TGet>(entity);
        }
    }
}