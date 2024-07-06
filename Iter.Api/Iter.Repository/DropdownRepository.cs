using AutoMapper;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class DropdownRepository : IDropdownRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public DropdownRepository(IterContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<LookupModel>> Get(int type, string? arrangementId  = null, string? agencyId = null, string? countryId = null)
        {
            var list = new List<LookupModel>();

            switch ((DropdownType)type)
            {
                case DropdownType.ReservationStatus:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.ReservationStatus.ToListAsync());

                case DropdownType.Agencies:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.Agency.ToListAsync());

                case DropdownType.Clients:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.Client.ToListAsync());

                case DropdownType.AccomodationTypes:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.ArrangementPrice.Where(x => x.ArrangementId == new Guid(arrangementId) && x.IsDeleted == false).ToListAsync());

                case DropdownType.Arrangments:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.Arrangement.Where(x => x.AgencyId == new Guid(agencyId) && x.IsDeleted == false).ToListAsync());

                case DropdownType.ArrangementStatus:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.ArrangementStatus.ToListAsync());

                case DropdownType.Employee:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.EmployeeArrangment.Where(x => x.Arrangement.AgencyId.ToString() == agencyId).ToListAsync());

                case DropdownType.Countries:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.Country.ToListAsync());

                case DropdownType.Cities:
                    return this.mapper.Map<List<LookupModel>>(await dbContext.City.Where(x => x.CountryId.ToString() == countryId).ToListAsync());

                default:
                    return new List<LookupModel>();

            }
        }
    }
}
