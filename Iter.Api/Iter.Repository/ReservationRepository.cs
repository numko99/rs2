using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Models;
using System.Reflection.Metadata.Ecma335;
using Iter.Core.Enum;

namespace Iter.Repository
{
    public class ReservationRepository : BaseCrudRepository<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>, IReservationRepository
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
           return await this.dbContext.Reservation
                    .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.Agency)
                    .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.ArrangementImages)
                        .ThenInclude(ai => ai.Image)
                    .Include(a => a.ArrangementPrice)
                    .Include(a => a.ReservationStatus)
                    .Include(a => a.Client)
                        .ThenInclude(a => a.User).Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public override async Task<PagedResult<ReservationSearchResponse>> Get(ReservationSearchModel? search)
        {
            var query = this.dbContext.Set<Reservation>().AsQueryable();

            PagedResult<ReservationSearchResponse> result = new PagedResult<ReservationSearchResponse>();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                query = query.Where(a => a.ReservationNumber.StartsWith(search.Name)
                || (a.Client.FirstName + " " + a.Client.LastName).StartsWith(search.Name)
                || (a.Client.LastName + " " + a.Client.FirstName).StartsWith(search.Name)
                || a.Arrangement.Name.StartsWith(search.Name));
            }

            if (!string.IsNullOrEmpty(search?.ReservationStatusId))
            {
                var statusId = int.Parse(search.ReservationStatusId);
                query = query.Where(a => a.ReservationStatusId == statusId).AsQueryable();
            }

            if (!string.IsNullOrEmpty(search?.AgencyId))
            {
                query = query.Where(a => a.Arrangement.AgencyId == new Guid(search.AgencyId)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(search?.UserId))
            {
                query = query.Where(a => a.Client != null ? a.Client.User.Id == search.UserId : false).AsQueryable();
            }

            if (!string.IsNullOrEmpty(search?.ArrangementId))
            {
                query = query.Where(a => a.ArrangmentId == new Guid(search.ArrangementId)).AsQueryable();
            }

            if (search?.ReturnActiveReservations != null && search?.ReturnActiveReservations.Value == true)
            {
                query = query.Where(a => (a.Arrangement.EndDate != null ? a.Arrangement.EndDate > DateTime.UtcNow : a.Arrangement.StartDate > DateTime.UtcNow)
                                         && (a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Pending || a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed)).AsQueryable();
            }

            if (search?.ReturnActiveReservations != null && search?.ReturnActiveReservations.Value == false)
            {
                query = query.Where(a => (a.Arrangement.EndDate != null ? a.Arrangement.EndDate < DateTime.UtcNow : a.Arrangement.StartDate < DateTime.UtcNow)
                                         || !(a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Pending || a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed)).AsQueryable();
            }

            query = query.Where(a => a.IsDeleted == false);

            query = query
                    .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.Agency)
                     .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.ArrangementImages)
                        .ThenInclude(ai => ai.Image)
                    .Include(a => a.ArrangementPrice)
                    .Include(a => a.Client)
                    .Include(a => a.ReservationStatus).AsNoTracking();

            result.Count = await query.CountAsync();

            query = query.OrderByDescending(q => q.CreatedAt);

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }
            result.Result = await query.Select(r => new ReservationSearchResponse()
            {
                ReservationId = r.Id,
                MainImage = this.mapper.Map<ImageResponse>(r.Arrangement.ArrangementImages.Where(a => a.IsMainImage).FirstOrDefault()),
                ArrangementName = r.Arrangement.Name,
                ArrangementId = r.Arrangement.Id,
                ArrangementStartDate = r.Arrangement.StartDate,
                ArrangementStartEndDate = r.Arrangement.EndDate,
                ReservationStatusId = r.ReservationStatusId,
                ReservationStatusName = r.ReservationStatus.Name,
                AgencyName = r.Arrangement.Agency.Name,
                ArrangementPrice = r.ArrangementPrice.Price,
                TotalPaid = r.TotalPaid,
                FirstName = r.Client.FirstName,
                LastName = r.Client.LastName,
                ReservationDate = r.CreatedAt,
                ReservationNumber = r.ReservationNumber
            }).ToListAsync();

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

        public async Task UpdateRatingAsync(Guid reservationId, int? newRating)
        {
            var reservation = await dbContext.Reservation
                                             .Include(r => r.Arrangement)
                                             .ThenInclude(a => a.Agency)
                                             .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null) { 
                throw new InvalidOperationException("Reservation not found.");
            }

            var arrangement = reservation.Arrangement;
            var agency = arrangement?.Agency;

            if (agency == null) throw new InvalidOperationException("Agency not found for the given reservation.");

            var agencyRatings = dbContext.Reservation
                                         .Where(r => r.Arrangement.AgencyId == agency.Id && r.Rating > 0);

            var agencyOldRating = await agencyRatings.AverageAsync(r => r.Rating);
            var agencyRatingCount = await agencyRatings.CountAsync();

            agency.Rating = CalculateNewAverage(agencyOldRating, agencyRatingCount, newRating.Value);

            var arrangementRatings = dbContext.Reservation
                                              .Where(r => r.Arrangement.Id == arrangement.Id && r.Rating > 0);

            var arrangementOldRating = await arrangementRatings.AverageAsync(r => r.Rating);
            var arrangementRatingCount = await arrangementRatings.CountAsync();

            arrangement.Rating = CalculateNewAverage(arrangementOldRating, arrangementRatingCount, newRating.Value);
            reservation.Rating = newRating.Value;

            await dbContext.SaveChangesAsync();
        }

        private decimal CalculateNewAverage(double oldRating, int count, int newRating)
        {
            return (decimal)((oldRating * count + newRating) / (count + 1));
        }
    }
}