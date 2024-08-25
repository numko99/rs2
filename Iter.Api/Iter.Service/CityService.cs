using AutoMapper;
using Iter.Model;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;
using Iter.Core.Models;
using Iter.Core.EntityModelss;

namespace Iter.Services
{
    public class CityService : BaseCrudService<City, CityUpsertRequest, CityUpsertRequest, CityResponse, CitySearchModel, CityResponse>, ICityService
    {
        private readonly ICityRepository cityRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CityService> logger;

        public CityService(ICityRepository cityRepository, IMapper mapper, ILogger<CityService> logger) : base(cityRepository, mapper, logger)
        {
            this.cityRepository = cityRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual async Task<PagedResult<CityResponse>> Get(CitySearchModel searchObject)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchObject}", searchObject);

            try
            {
                int? countryId = null;

                if (int.TryParse(searchObject.CountryId, out var countryIdTemp))
                {
                    countryId = countryIdTemp;
                }

                var searchData = await this.cityRepository.Get(searchObject.PageSize, searchObject.CurrentPage, searchObject.Name, countryId);
                var result = this.mapper.Map<PagedResult<CityResponse>>(searchData);

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchObject}", searchObject);
                throw;
            }
        }

        public virtual async Task<CityResponse> GetById(int id)
        {
            logger.LogInformation("GetById operation started for City ID: {Id}", id);

            try
            {
                var entity = await this.cityRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("City with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid request");
                }

                var result = this.mapper.Map<CityResponse>(entity);

                logger.LogInformation("GetById operation completed successfully for City ID: {Id}", id);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for City ID: {Id}", id);
                throw;
            }
        }

        public async Task Update(int id, CityUpsertRequest request)
        {
            logger.LogInformation("Update operation started for City ID: {Id}", id);

            try
            {
                if (request == null)
                {
                    logger.LogWarning("Invalid request for Update operation.");
                    throw new ArgumentException("Invalid request");
                }

                var entity = await this.cityRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("City with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid id");
                }

                this.mapper.Map(request, entity);
                await this.cityRepository.UpdateAsync(entity);

                logger.LogInformation("Update operation completed successfully for City ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Update operation for City ID: {Id}", id);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            logger.LogInformation("Delete operation started for City ID: {Id}", id);

            try
            {
                var entity = await this.cityRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("City with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid id");
                }

                await this.cityRepository.DeleteAsync(entity);

                logger.LogInformation("Delete operation completed successfully for City ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Delete operation for City ID: {Id}", id);
                throw;
            }
        }
    }
}
