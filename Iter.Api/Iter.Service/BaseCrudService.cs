using AutoMapper;
using Iter.Core;
using Iter.Core.RequestParameterModels;
using Iter.Repository;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class BaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> : BaseReadService<T, TGet, TSearchRequest, TSearchResponse>
        where TInsert : class
        where T : class
        where TGet : class
        where TSearchResponse : class
        where TSearchRequest : BaseSearchModel
    {
        private readonly IBaseCrudRepository<T> baseCrudRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse>> logger;

        public BaseCrudService(IBaseCrudRepository<T> baseCrudRepository, IMapper mapper, ILogger<BaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse>> logger)
            : base(baseCrudRepository, mapper, logger)
        {
            this.baseCrudRepository = baseCrudRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual async Task Delete(Guid id)
        {
            logger.LogInformation("Delete operation started for ID: {Id}", id);

            try
            {
                var entity = await this.baseCrudRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new ArgumentException($"Entity with id {id} not found.");
                }

                await this.baseCrudRepository.DeleteAsync(entity);
                logger.LogInformation("Delete operation completed successfully for ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Delete operation for ID: {Id}", id);
                throw;
            }
        }

        public virtual async Task Insert(TInsert request)
        {
            logger.LogInformation("Insert operation started.");

            try
            {
                var entity = this.mapper.Map<T>(request);
                await this.baseCrudRepository.AddAsync(entity);
                logger.LogInformation("Insert operation completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Insert operation.");
                throw;
            }
        }

        public virtual async Task Update(Guid id, TUpdate request)
        {
            logger.LogInformation("Update operation started for ID: {Id}", id);

            try
            {
                if (request == null)
                {
                    logger.LogWarning("Invalid request for Update operation.");
                    throw new ArgumentException("Invalid request");
                }

                var entity = await this.baseCrudRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("Entity with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid id");
                }

                this.mapper.Map(request, entity);
                await this.baseCrudRepository.UpdateAsync(entity);
                logger.LogInformation("Update operation completed successfully for ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Update operation for ID: {Id}", id);
                throw;
            }
        }
    }
}
