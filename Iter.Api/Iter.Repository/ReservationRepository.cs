using Iter.Core.EntityModels;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Models;
using Iter.Core.Dto;
using Iter.Core.RequestParameterModels;
using System.Globalization;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Iter.Core;

namespace Iter.Repository
{
    public class ReservationRepository : BaseCrudRepository<Reservation>, IReservationRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ReservationRepository> logger;

        public ReservationRepository(IterContext context, ILogger<ReservationRepository> logger, IMapper mapper) : base(context, logger)
        {
            this.dbContext = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<string> GetLatestReservationNumber()
        {
            logger.LogInformation("GetLatestReservationNumber operation started.");

            try
            {
                var reservation = await dbContext.Reservation.OrderByDescending(x => x.ReservationNumber).FirstOrDefaultAsync();

                var latestNumber = reservation == null ? "1000" : reservation.ReservationNumber;

                logger.LogInformation("GetLatestReservationNumber operation completed. Latest reservation number: {LatestNumber}", latestNumber);

                return latestNumber;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetLatestReservationNumber operation.");
                throw;
            }
        }

        public async override Task<Reservation?> GetById(Guid id)
        {
            logger.LogInformation("GetById operation started for Reservation ID: {Id}", id);

            try
            {
                var reservation = await dbContext.Reservation
                    .Include(a => a.DepartureCity)
                    .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.Agency)
                    .Include(a => a.Arrangement)
                        .ThenInclude(ai => ai.ArrangementImages)
                        .ThenInclude(ai => ai.Image)
                    .Include(a => a.ArrangementPrice)
                    .Include(a => a.ReservationStatus)
                    .Include(a => a.Client)
                        .ThenInclude(a => a.User)
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();

                if (reservation == null)
                {
                    logger.LogWarning("No reservation found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for Reservation ID: {Id}", id);
                }

                return reservation;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for Reservation ID: {Id}", id);
                throw;
            }
        }

        public async Task<PagedResult<ReservationSearchDto>> Get(ReservationSearchRequesParameters? search)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchParameters}", search);

            try
            {
                var query = dbContext.Set<Reservation>().AsQueryable();
                var result = new PagedResult<ReservationSearchDto>();

                if (!string.IsNullOrEmpty(search?.Name))
                {
                    query = query.Where(a => a.ReservationNumber.Contains(search.Name)
                    || (a.Client.FirstName + " " + a.Client.LastName).Contains(search.Name)
                    || (a.Client.LastName + " " + a.Client.FirstName).Contains(search.Name)
                    || a.Arrangement.Name.Contains(search.Name));
                }

                if (!string.IsNullOrEmpty(search?.ReservationStatusId))
                {
                    var statusId = int.Parse(search.ReservationStatusId);
                    query = query.Where(a => a.ReservationStatusId == statusId);
                }

                if (!string.IsNullOrEmpty(search.DateFrom))
                {
                    var dateFrom = DateTime.ParseExact(search.DateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(a => dateFrom < a.CreatedAt);
                }

                if (!string.IsNullOrEmpty(search.DateTo))
                {
                    var dateTo = DateTime.ParseExact(search.DateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(a => dateTo > a.CreatedAt);
                }

                if (!string.IsNullOrEmpty(search?.AgencyId))
                {
                    query = query.Where(a => a.Arrangement.AgencyId == new Guid(search.AgencyId));
                }

                if (!string.IsNullOrEmpty(search?.UserId))
                {
                    query = query.Where(a => a.Client != null ? a.Client.User.Id == search.UserId : false);
                }

                if (!string.IsNullOrEmpty(search?.ArrangementId))
                {
                    query = query.Where(a => a.ArrangmentId == new Guid(search.ArrangementId));
                }

                if (search?.ReturnActiveReservations != null && search.ReturnActiveReservations.Value == true)
                {
                    query = query.Where(a => (a.Arrangement.EndDate != null ? a.Arrangement.EndDate > DateTime.UtcNow : a.Arrangement.StartDate > DateTime.UtcNow)
                                             && (a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Pending || a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed));
                }

                if (search?.ReturnActiveReservations != null && search.ReturnActiveReservations.Value == false)
                {
                    query = query.Where(a => (a.Arrangement.EndDate != null ? a.Arrangement.EndDate < DateTime.UtcNow : a.Arrangement.StartDate < DateTime.UtcNow)
                                             || !(a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Pending || a.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed));
                }

                query = query.Where(a => a.IsDeleted == false);

                result.Count = await query.CountAsync();

                query = query
                        .Include(a => a.Arrangement)
                            .ThenInclude(ai => ai.Agency)
                         .Include(a => a.Arrangement)
                            .ThenInclude(ai => ai.ArrangementImages)
                            .ThenInclude(ai => ai.Image)
                        .Include(a => a.ArrangementPrice)
                        .Include(a => a.Client)
                        .Include(a => a.ReservationStatus).AsNoTracking();

                query = query.OrderByDescending(q => q.CreatedAt);

                if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
                {
                    query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
                }

                result.Result = await query.Select(r => new ReservationSearchDto()
                {
                    ReservationId = r.Id,
                    MainImage = mapper.Map<ImageDto>(r.Arrangement.ArrangementImages.Where(a => a.IsMainImage).FirstOrDefault()),
                    ArrangementName = r.Arrangement.Name,
                    ArrangementId = r.Arrangement.Id,
                    ArrangementStartDate = r.Arrangement.StartDate,
                    ArrangementStartEndDate = r.Arrangement.EndDate,
                    ReservationStatusId = r.ReservationStatusId,
                    ReservationStatusName = r.ReservationStatus.Name,
                    AgencyName = r.Arrangement.Agency.Name,
                    ArrangementPrice = r.ArrangementPrice.Price,
                    TotalPaid = r.TotalPaid,
                    FirstName = r.Client.FirstName ?? "",
                    LastName = r.Client.LastName ?? "",
                    UserId = r.Client.User.Id,
                    ReservationDate = r.CreatedAt,
                    ReservationNumber = r.ReservationNumber
                }).ToListAsync();

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchParameters}", search);
                throw;
            }
        }

        public async override Task DeleteAsync(Reservation entity)
        {
            logger.LogInformation("DeleteAsync operation started for Reservation ID: {Id}", entity.Id);

            try
            {
                entity.IsDeleted = true;
                dbContext.Reservation.Update(entity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("DeleteAsync operation completed successfully for Reservation ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during DeleteAsync operation for Reservation ID: {Id}", entity.Id);
                throw;
            }
        }

        public async Task<int> GetCount(Guid arrangementId)
        {
            logger.LogInformation("GetCount operation started for Arrangement ID: {ArrangementId}", arrangementId);

            try
            {
                var count = await dbContext.Reservation.Where(r => r.ArrangmentId == arrangementId).CountAsync();

                logger.LogInformation("GetCount operation completed successfully with count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCount operation for Arrangement ID: {ArrangementId}", arrangementId);
                throw;
            }
        }

        public async Task UpdateRatingAsync(Guid reservationId, int? newRating)
        {
            logger.LogInformation("UpdateRatingAsync operation started for Reservation ID: {ReservationId} with new rating: {NewRating}", reservationId, newRating);

            try
            {
                var reservation = await dbContext.Reservation
                                                 .Include(r => r.Arrangement)
                                                 .ThenInclude(a => a.Agency)
                                                 .FirstOrDefaultAsync(r => r.Id == reservationId);

                if (reservation == null)
                {
                    logger.LogWarning("Reservation not found for ID: {ReservationId} during UpdateRatingAsync operation.", reservationId);
                    throw new InvalidOperationException("Reservation not found.");
                }

                var arrangement = reservation.Arrangement;
                var agency = arrangement?.Agency;

                if (agency == null)
                {
                    logger.LogWarning("Agency not found for Reservation ID: {ReservationId} during UpdateRatingAsync operation.", reservationId);
                    throw new InvalidOperationException("Agency not found for the given reservation.");
                }

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

                logger.LogInformation("UpdateRatingAsync operation completed successfully for Reservation ID: {ReservationId}.", reservationId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during UpdateRatingAsync operation for Reservation ID: {ReservationId}", reservationId);
                throw;
            }
        }

        public async Task<List<Reservation>> GetArrangementsByDestinationCityNames(List<int> cities)
        {
            logger.LogInformation("GetArrangementsByDestinationCityNames operation started with city IDs: {@Cities}", cities);

            try
            {
                var clientIds = await dbContext.Reservation
                                    .Where(d => d.Arrangement.Destinations.Any(x => cities.Contains(x.CityId)))
                                    .Select(x => x.ClientId)
                                    .ToListAsync();

                var reservations = await dbContext.Reservation
                                                  .Include(x => x.Arrangement)
                                                  .ThenInclude(x => x.Destinations)
                                                  .Where(r => clientIds.Contains(r.ClientId))
                                                  .ToListAsync();

                logger.LogInformation("GetArrangementsByDestinationCityNames operation completed successfully with {Count} reservations.", reservations.Count);

                return reservations;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetArrangementsByDestinationCityNames operation with city IDs: {@Cities}", cities);
                throw;
            }
        }

        private decimal CalculateNewAverage(double oldRating, int count, int newRating)
        {
            return (decimal)((oldRating * count + newRating) / (count + 1));
        }

        public async override Task<IEnumerable<Reservation>> GetAll()
        {
            logger.LogInformation("GetAll operation started.");

            try
            {
                var reservations = await dbContext.Reservation
                                                  .Include(x => x.Arrangement)
                                                  .ThenInclude(x => x.Destinations)
                                                  .ToListAsync();

                logger.LogInformation("GetAll operation completed successfully with {Count} reservations.", reservations.Count);

                return reservations;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetAll operation.");
                throw;
            }
        }

        public async Task<int> GetCount(Guid? agencyId = null)
        {
            logger.LogInformation("GetCount operation started for Agency ID: {AgencyId}", agencyId);

            try
            {
                var count = await dbContext.Reservation.Where(r => agencyId == null || r.Arrangement.AgencyId == agencyId).CountAsync();

                logger.LogInformation("GetCount operation completed successfully with count: {Count}", count);

                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCount operation for Agency ID: {AgencyId}", agencyId);
                throw;
            }
        }

        public async Task<decimal> GetTotalAmount(Guid? agencyId = null)
        {
            logger.LogInformation("GetTotalAmount operation started for Agency ID: {AgencyId}", agencyId);

            try
            {
                var totalAmount = await dbContext.Reservation.Where(r => agencyId == null || r.Arrangement.AgencyId == agencyId).SumAsync(x => x.TotalPaid);

                logger.LogInformation("GetTotalAmount operation completed successfully with total amount: {TotalAmount}", totalAmount);

                return totalAmount;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetTotalAmount operation for Agency ID: {AgencyId}", agencyId);
                throw;
            }
        }
    }
}
