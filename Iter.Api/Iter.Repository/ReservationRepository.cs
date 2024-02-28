using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace Iter.Repository
{
    public class ReservationRepository : BaseCrudRepository<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel>, IReservationRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;
        public ReservationRepository(IterContext context, IMapper mapper) : base(context, mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        public async Task<string> GetLatestReservationNumber()
        {
            var reservation = await this.dbContext.Reservation.OrderByDescending(x => x.ReservationNumber).FirstOrDefaultAsync();

            if (reservation == null)
            {
                return "1000";
            }

            return reservation.ReservationNumber;
        }

        public async override Task<Reservation?> GetById(Guid id)
        {
           return await this.dbContext.Reservation.Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.Agency)
                    .Include(a => a.ArrangementPrice)
                    .Include(a => a.ReservationStatus)
                    .Include(a => a.Client).Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<PagedResult<ReservationResponse>> Get(ReservationSearchModel? search)
        {
            var query = this.dbContext.Set<Reservation>().AsQueryable();

            PagedResult<ReservationResponse> result = new PagedResult<ReservationResponse>();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                query = query.Where(a => a.ReservationNumber.StartsWith(search.Name)
                || $"{a.Client.FirstName} {a.Client.LastName}".StartsWith(search.Name)
                || a.Arrangement.Name.StartsWith(search.Name));
            }

            if (!string.IsNullOrEmpty(search?.AgencyId))
            {
                query = query.Where(a => a.Arrangement.AgencyId == new Guid(search.AgencyId)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(search?.ArrangementId))
            {
                query = query.Where(a => a.ArrangmentId == new Guid(search.ArrangementId)).AsQueryable();
            }

            query = query.Where(a => a.IsDeleted == false);

            query = query
                    .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.Agency)
                    .Include(a => a.ArrangementPrice)
                    .Include(a => a.Client)
                    .Include(a => a.ReservationStatus);

            result.Count = await query.CountAsync();

            query = query.OrderByDescending(q => q.CreatedAt);

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }
            var list = await query.ToListAsync();
            var tmp = this.mapper.Map<List<ReservationResponse>>(list);

            result.Result = tmp;
            return result;
        }

        public async override Task DeleteAsync(Reservation entity)
        {
            entity.IsDeleted = true;
            this.dbContext.Reservation.Update(entity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<int> GetCount(Guid arrangementId)
        {
            var count = await this.dbContext.Reservation.Where(r => r.ArrangmentId == arrangementId).CountAsync();
            return count;
        }
    }
}