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
        public async Task<PagedResult<DropdownModel>> Get(DropdownType type)
        {
            var list = new List<DropdownModel>();

            switch (type)
            {
                case DropdownType.ReservationStatus:
                    list = this.mapper.Map<List<DropdownModel>>(await dbContext.ReservationStatus.ToListAsync());
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
