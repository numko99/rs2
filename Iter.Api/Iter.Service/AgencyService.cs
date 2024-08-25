using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class AgencyService : BaseCrudService<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel, AgencyResponse>, IAgencyService
    {
        private readonly IAgencyRepository agencyRepository;
        private readonly IMapper mapper;
        private readonly ILogger<AgencyService> logger;

        public AgencyService(IAgencyRepository agencyRepository, IMapper mapper, ILogger<AgencyService> logger) : base(agencyRepository, mapper, logger)
        {
            this.agencyRepository = agencyRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<PagedResult<AgencyResponse>> Get(AgencySearchModel searchObject)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchObject}", searchObject);

            try
            {
                var searchData = await this.agencyRepository.Get(searchObject.Name, searchObject.PageSize, searchObject.CurrentPage);
                var result = this.mapper.Map<PagedResult<AgencyResponse>>(searchData);

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchObject}", searchObject);
                throw;
            }
        }

        public override async Task Update(Guid id, AgencyInsertRequest request)
        {
            logger.LogInformation("Update operation started for Agency ID: {Id}", id);

            try
            {
                var agency = await agencyRepository.GetById(id);

                if (agency == null)
                {
                    logger.LogWarning("Agency with ID {Id} not found.", id);
                    throw new ArgumentException("Agency not found.");
                }

                this.mapper.Map(request, agency);

                if (request.Logo == null)
                {
                    agency.ImageId = null;
                }

                await this.agencyRepository.UpdateAsync(agency);

                logger.LogInformation("Update operation completed successfully for Agency ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Update operation for Agency ID: {Id}", id);
                throw;
            }
        }

        public async Task<AgencyResponse?> GetByEmployeeId(Guid employeeId)
        {
            logger.LogInformation("GetByEmployeeId operation started for Employee ID: {EmployeeId}", employeeId);

            try
            {
                var agency = await this.agencyRepository.GetByEmployeeId(employeeId);

                if (agency == null)
                {
                    logger.LogWarning("No agency found for Employee ID: {EmployeeId}", employeeId);
                    return null;
                }

                var result = this.mapper.Map<AgencyResponse>(agency);

                logger.LogInformation("GetByEmployeeId operation completed successfully for Employee ID: {EmployeeId}", employeeId);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetByEmployeeId operation for Employee ID: {EmployeeId}", employeeId);
                throw;
            }
        }
    }
}
