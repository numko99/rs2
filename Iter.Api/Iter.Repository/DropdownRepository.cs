using AutoMapper;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class DropdownRepository : IDropdownRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<DropdownRepository> logger;

        public DropdownRepository(IterContext dbContext, IMapper mapper, ILogger<DropdownRepository> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<List<LookupModel>> Get(int type, string? arrangementId = null, string? agencyId = null, string? countryId = null)
        {
            logger.LogInformation("Get operation started for dropdown type: {DropdownType} with arrangementId={ArrangementId}, agencyId={AgencyId}, countryId={CountryId}", type, arrangementId, agencyId, countryId);

            try
            {
                var list = new List<LookupModel>();

                switch ((DropdownType)type)
                {
                    case DropdownType.ReservationStatus:
                        logger.LogInformation("Fetching ReservationStatus dropdown data.");
                        return mapper.Map<List<LookupModel>>(await dbContext.ReservationStatus.ToListAsync());

                    case DropdownType.Agencies:
                        logger.LogInformation("Fetching Agencies dropdown data.");
                        return mapper.Map<List<LookupModel>>(await dbContext.Agency.Where(a => !a.IsDeleted).ToListAsync());

                    case DropdownType.Clients:
                        logger.LogInformation("Fetching Clients dropdown data.");
                        return mapper.Map<List<LookupModel>>(await dbContext.Client.Where(a => !a.IsDeleted).ToListAsync());

                    case DropdownType.AccomodationTypes:
                        logger.LogInformation("Fetching AccomodationTypes dropdown data for arrangementId: {ArrangementId}.", arrangementId);
                        return mapper.Map<List<LookupModel>>(await dbContext.ArrangementPrice.Where(x => x.ArrangementId == new Guid(arrangementId) && x.IsDeleted == false).ToListAsync());

                    case DropdownType.Arrangments:
                        logger.LogInformation("Fetching Arrangments dropdown data for agencyId: {AgencyId}.", agencyId);
                        return mapper.Map<List<LookupModel>>(await dbContext.Arrangement.Where(x => x.AgencyId == new Guid(agencyId) && x.IsDeleted == false).ToListAsync());

                    case DropdownType.ArrangementStatus:
                        logger.LogInformation("Fetching ArrangementStatus dropdown data.");
                        return mapper.Map<List<LookupModel>>(await dbContext.ArrangementStatus.ToListAsync());

                    case DropdownType.Employee:
                        logger.LogInformation("Fetching Employee dropdown data for agencyId: {AgencyId}.", agencyId);
                        return mapper.Map<List<LookupModel>>(await dbContext.EmployeeArrangment.Where(x => x.Arrangement.AgencyId.ToString() == agencyId).ToListAsync());

                    case DropdownType.Countries:
                        logger.LogInformation("Fetching Countries dropdown data.");
                        return mapper.Map<List<LookupModel>>(await dbContext.Country.ToListAsync());

                    case DropdownType.Cities:
                        logger.LogInformation("Fetching Cities dropdown data for countryId: {CountryId}.", countryId);
                        return mapper.Map<List<LookupModel>>(await dbContext.City.Where(x => x.CountryId.ToString() == countryId).ToListAsync());

                    default:
                        logger.LogWarning("Unknown dropdown type: {DropdownType}. Returning empty list.", type);
                        return new List<LookupModel>();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation for dropdown type: {DropdownType} with arrangementId={ArrangementId}, agencyId={AgencyId}, countryId={CountryId}", type, arrangementId, agencyId, countryId);
                throw;
            }
        }
    }
}
