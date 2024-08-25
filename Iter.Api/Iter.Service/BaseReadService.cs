using AutoMapper;
using Iter.Core;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Repository;
using Iter.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class BaseReadService<T, TGet, TSearchRequest, TSearchResponse> : IBaseReadService<T, TGet, TSearchRequest, TSearchResponse>
        where T : class
        where TGet : class
        where TSearchResponse : class
        where TSearchRequest : BaseSearchModel
    {
        protected readonly IBaseReadRepository<T> baseReadRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BaseReadService<T, TGet, TSearchRequest, TSearchResponse>> logger;

        public BaseReadService(IBaseReadRepository<T> baseReadRepository, IMapper mapper, ILogger<BaseReadService<T, TGet, TSearchRequest, TSearchResponse>> logger)
        {
            this.baseReadRepository = baseReadRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual Task<PagedResult<TSearchResponse>> Get(TSearchRequest searchObject)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchParameters}", searchObject);

            // TODO: Implement the actual Get logic
            throw new NotImplementedException();
        }

        public virtual async Task<List<TGet>> GetAll()
        {
            logger.LogInformation("GetAll operation started.");

            try
            {
                var entities = await this.baseReadRepository.GetAll();
                var result = this.mapper.Map<List<TGet>>(entities);

                logger.LogInformation("GetAll operation completed successfully with {Count} entities.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetAll operation.");
                throw;
            }
        }

        public virtual async Task<TGet> GetById(Guid id)
        {
            logger.LogInformation("GetById operation started for ID: {Id}", id);

            try
            {
                var entity = await this.baseReadRepository.GetById(id);
                if (entity == null)
                {
                    logger.LogWarning("No entity found for ID: {Id}", id);
                    throw new ArgumentException("Invalid request");
                }

                var result = this.mapper.Map<TGet>(entity);

                logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for ID: {Id}", id);
                throw;
            }
        }
    }
}
