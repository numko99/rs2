using AutoMapper;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

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
        public async Task<PagedResult<DropdownModel>> Get(int type, string? arrangementId  = null, string? agencyId = null)
        {
            var list = new List<DropdownModel>();

            switch ((DropdownType)type)
            {
                case DropdownType.ReservationStatus:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.ReservationStatus.ToListAsync());
                    break;
                case DropdownType.Agencies:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.Agency.ToListAsync());
                    break;
                case DropdownType.Clients:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.Client.ToListAsync());
                    break;
                case DropdownType.AccomodationTypes:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.ArrangementPrice.Where(x => x.ArrangementId == new Guid(arrangementId) && x.IsDeleted == false).ToListAsync());
                    break;
                case DropdownType.Arrangments:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.Arrangement.Where(x => x.AgencyId == new Guid(agencyId) && x.IsDeleted == false).ToListAsync());
                    break;
                case DropdownType.ArrangementStatus:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.ArrangementStatus.ToListAsync());
                    break;
                case DropdownType.Employee:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.EmployeeArrangment.Where(x => x.Arrangement.AgencyId.ToString() == agencyId).ToListAsync());
                    break;
                default:
                    break;
            }

            return new PagedResult<DropdownModel>()
            {
                Result = list,
                Count = list.Count
            };
        }
    }
}
