using AutoMapper;
using Iter.Model;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;
using Iter.Core.Models;
using Iter.Core.EntityModelss;

namespace Iter.Services
{
    public class CountryService : BaseCrudService<Country, CountryUpsertRequest, CountryUpsertRequest, CountryResponse, CitySearchModel, CountryResponse>, ICountryService
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CountryService> logger;

        public CountryService(ICountryRepository countryRepository, IMapper mapper, ILogger<CountryService> logger) : base(countryRepository, mapper, logger)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual async Task<PagedResult<CountryResponse>> Get(CitySearchModel searchObject)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchObject}", searchObject);

            try
            {
                var searchData = await this.countryRepository.Get(searchObject.PageSize, searchObject.CurrentPage, searchObject.Name);
                var result = this.mapper.Map<PagedResult<CountryResponse>>(searchData);

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchObject}", searchObject);
                throw;
            }
        }

        public virtual async Task<CountryResponse> GetById(int id)
        {
            logger.LogInformation("GetById operation started for Country ID: {Id}", id);

            try
            {
                var entity = await this.countryRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("Country with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid request");
                }

                var result = this.mapper.Map<CountryResponse>(entity);

                logger.LogInformation("GetById operation completed successfully for Country ID: {Id}", id);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for Country ID: {Id}", id);
                throw;
            }
        }

        public async Task Update(int id, CountryUpsertRequest request)
        {
            logger.LogInformation("Update operation started for Country ID: {Id}", id);

            try
            {
                if (request == null)
                {
                    logger.LogWarning("Invalid request for Update operation.");
                    throw new ArgumentException("Invalid request");
                }

                var entity = await this.countryRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("Country with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid id");
                }

                this.mapper.Map(request, entity);
                await this.countryRepository.UpdateAsync(entity);

                logger.LogInformation("Update operation completed successfully for Country ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Update operation for Country ID: {Id}", id);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            logger.LogInformation("Delete operation started for Country ID: {Id}", id);

            try
            {
                var entity = await this.countryRepository.GetById(id);

                if (entity == null)
                {
                    logger.LogWarning("Country with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid id");
                }

                await this.countryRepository.DeleteAsync(entity);

                logger.LogInformation("Delete operation completed successfully for Country ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Delete operation for Country ID: {Id}", id);
                throw;
            }
        }
    }
}
