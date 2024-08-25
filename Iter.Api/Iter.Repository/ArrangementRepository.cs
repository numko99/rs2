using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Iter.Core.Dto;
using Iter.Core.RequestParameterModels;

namespace Iter.Repository
{
    public class ArrangementRepository : BaseCrudRepository<Arrangement>, IArrangementRepository
    {
        private readonly IterContext dbContext;
        private readonly ILogger<ArrangementRepository> logger;
        private readonly IMapper mapper;

        public ArrangementRepository(IterContext dbContext, ILogger<ArrangementRepository> logger, IMapper mapper) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<PagedResult<ArrangementSearchDto>> Get(ArrangmentSearchParameters? search)
        {
            logger.LogInformation("Get operation started with search parameters: {@Search}", search);

            try
            {
                var query = dbContext.Set<Arrangement>().AsQueryable();

                if (!string.IsNullOrEmpty(search?.Name))
                {
                    query = query.Where(a => a.Name.Contains(search.Name) || a.Destinations.Any(x => x.City.Name.Contains(search.Name))).AsQueryable();
                }

                if (!string.IsNullOrEmpty(search?.AgencyId))
                {
                    var guid = new Guid(search.AgencyId);
                    query = query.Where(a => a.AgencyId == guid).AsQueryable();
                }

                if (search?.DateFrom != null)
                {
                    var dateFrom = DateTime.ParseExact(search.DateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(a => a.StartDate.Date >= dateFrom.Date).AsQueryable();
                }

                if (search?.DateTo != null)
                {
                    var dateTo = DateTime.ParseExact(search.DateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(a => dateTo.Date >= a.StartDate.Date).AsQueryable();
                }

                if (search?.ArrangementStatus != null)
                {
                    query = query.Where(a => a.ArrangementStatusId == search.ArrangementStatus).AsQueryable();
                }

                if (search?.Rating != null)
                {
                    query = query.Where(a => a.Agency.Rating >= search.Rating).AsQueryable();
                }

                query = query.Where(q => q.IsDeleted == false);

                var count = await query.CountAsync();

                if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
                {
                    query = query.OrderByDescending(q => q.CreatedAt).Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
                }

                query = query.Include(a => a.Agency)
                             .Include(a => a.ArrangementStatus)
                             .Include(a => a.ArrangementImages)
                             .ThenInclude(a => a.Image);

                var result = await query.Select(a => new ArrangementSearchDto
                {
                    Id = a.Id,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Name = a.Name,
                    AgencyName = a.Agency.Name,
                    AgencyRating = a.Agency.Rating,
                    ArrangementStatusId = a.ArrangementStatusId,
                    ArrangementStatusName = a.ArrangementStatus.Name,
                    MainImage = this.mapper.Map<ImageDto>(a.ArrangementImages.Where(a => a.IsMainImage).FirstOrDefault()),
                    IsReserved = search.CurrentUserId != null ? this.dbContext.Reservation.Any(r => r.ArrangmentId == a.Id && r.ClientId == search.CurrentUserId && r.IsDeleted == false && (r.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed || r.ReservationStatusId == (int)Core.Enum.ReservationStatus.Pending)) : false
                }).ToListAsync();

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Count);

                return new PagedResult<ArrangementSearchDto>
                {
                    Count = count,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@Search}", search);
                throw;
            }
        }

        public override async Task<Arrangement?> GetById(Guid id)
        {
            logger.LogInformation("GetById operation started for ID: {Id}", id);

            try
            {
                var arrangement = await dbContext.Arrangement
                    .Include(a => a.Agency)
                        .ThenInclude(a => a.Image)
                    .Include(a => a.Agency)
                        .ThenInclude(a => a.Address)
                        .ThenInclude(a => a.City)
                    .Include(a => a.ArrangementStatus)
                    .Include(a => a.EmployeeArrangments)
                        .ThenInclude(a => a.Employee)
                    .Include(a => a.ArrangementPrices)
                    .Include(a => a.ArrangementImages)
                        .ThenInclude(ai => ai.Image)
                    .Include(a => a.Destinations)
                        .ThenInclude(ai => ai.City)
                        .ThenInclude(ai => ai.Country)
                    .Include(a => a.Destinations)
                        .ThenInclude(ai => ai.Accommodation)
                        .ThenInclude(ai => ai.HotelAddress)
                    .Where(a => a.Id == id && a.IsDeleted == false)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (arrangement == null)
                {
                    logger.LogWarning("GetById operation completed: No arrangement found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);
                }

                return arrangement;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for ID: {Id}", id);
                throw;
            }
        }

        public async Task SmartUpdateAsync(Arrangement entity)
        {
            logger.LogInformation("SmartUpdateAsync operation started for Arrangement ID: {Id}", entity.Id);

            try
            {
                if (entity.ArrangementStatusId == (int)Core.Enum.ArrangementStatus.InPreparation)
                {
                    var pricesIds = entity.ArrangementPrices.Select(a => a.Id).ToList();
                    var pricesToDelete = dbContext.ArrangementPrice
                       .Where(ap => ap.ArrangementId == entity.Id && !pricesIds.Contains(ap.Id))
                       .ToList();
                    dbContext.ArrangementPrice.RemoveRange(pricesToDelete);
                }

                var existingImages = dbContext.ArrangementImage
                    .Where(ai => ai.ArrangementId == entity.Id)
                    .ToList();

                var imageIds = existingImages.Select(ai => ai.ImageId).ToList();
                var imagesToDelete = dbContext.Image
                    .Where(i => imageIds.Contains(i.Id))
                    .ToList();

                var destinationIds = entity.Destinations.Select(des => des.Id).ToList();
                var destinationsToDelete = dbContext.Destination
                    .Where(d => d.ArrangementId == entity.Id && !destinationIds.Contains(d.Id)).AsNoTracking()
                    .ToList();

                dbContext.ArrangementImage.RemoveRange(existingImages.ToList());
                dbContext.Image.RemoveRange(imagesToDelete.ToList());
                dbContext.Destination.RemoveRange(destinationsToDelete.ToList());
                await dbContext.SaveChangesAsync();

                foreach (var destination in entity.Destinations)
                {
                    var existingDestination = dbContext.Destination.AsNoTracking().FirstOrDefault(d => d.Id == destination.Id);
                    if (existingDestination != null)
                    {
                        dbContext.Entry(destination).State = EntityState.Modified;
                    }
                    else
                    {
                        dbContext.Destination.Add(destination);
                    }
                }

                if (entity.ArrangementStatusId == (int)Core.Enum.ArrangementStatus.InPreparation)
                {
                    foreach (var price in entity.ArrangementPrices)
                    {
                        var existingPrice = dbContext.ArrangementPrice.AsNoTracking().FirstOrDefault(d => d.Id == price.Id);
                        if (existingPrice != null)
                        {
                            dbContext.ArrangementPrice.Update(price);
                        }
                        else
                        {
                            dbContext.ArrangementPrice.Add(price);
                        }
                    }
                }

                dbContext.ArrangementImage.AddRange(entity.ArrangementImages);
                dbContext.Arrangement.Update(entity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("SmartUpdateAsync operation completed successfully for Arrangement ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during SmartUpdateAsync operation for Arrangement ID: {Id}", entity.Id);
                throw;
            }
        }

        public override async Task DeleteAsync(Arrangement entity)
        {
            logger.LogInformation("DeleteAsync operation started for Arrangement ID: {Id}", entity.Id);

            try
            {
                entity.IsDeleted = true;
                foreach (var destination in entity.Destinations)
                {
                    destination.IsDeleted = true;
                }

                foreach (var price in entity.ArrangementPrices)
                {
                    price.IsDeleted = true;
                }

                foreach (var image in entity.ArrangementImages)
                {
                    image.IsDeleted = true;
                }

                dbContext.Arrangement.Update(entity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("DeleteAsync operation completed successfully for Arrangement ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during DeleteAsync operation for Arrangement ID: {Id}", entity.Id);
                throw;
            }
        }

        public async Task<ArrangementPrice> GetArrangementPriceAsync(Guid id)
        {
            logger.LogInformation("GetArrangementPriceAsync operation started for ArrangementPrice ID: {Id}", id);

            try
            {
                var arrangementPrice = await dbContext.ArrangementPrice.Where(a => a.Id == id).FirstOrDefaultAsync();

                if (arrangementPrice == null)
                {
                    logger.LogWarning("GetArrangementPriceAsync operation completed: No ArrangementPrice found for ID: {Id}", id);
                }
                else
                {
                    logger.LogInformation("GetArrangementPriceAsync operation completed successfully for ArrangementPrice ID: {Id}", id);
                }

                return arrangementPrice;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetArrangementPriceAsync operation for ArrangementPrice ID: {Id}", id);
                throw;
            }
        }

        public async Task<List<ArrangementSearchDto>> GetRecommendedArrangementsByDestinationNames(List<int> cities, Guid? clientId)
        {
            logger.LogInformation("GetRecommendedArrangementsByDestinationNames operation started with cities: {@Cities} and clientId: {ClientId}", cities, clientId);

            try
            {
                var arrangements = new List<ArrangementSearchDto>();
                foreach (var cityId in cities)
                {
                    var arrangement = await dbContext.Arrangement
                        .Include(a => a.ArrangementImages)
                        .ThenInclude(a => a.Image)
                        .Include(a => a.Agency)
                        .Include(a => a.ArrangementPrices)
                        .Where(a => a.Destinations.Any(x => x.CityId == cityId) && a.ArrangementStatusId == (int)Core.Enum.ArrangementStatus.AvailableForReservation)
                        .FirstOrDefaultAsync();

                    if (arrangement == null)
                    {
                        logger.LogWarning("No arrangement found for city ID: {CityId}", cityId);
                        continue;
                    }

                    if (!arrangements.Any(a => a.Id == arrangement.Id))
                    {
                        arrangements.Add(new ArrangementSearchDto
                        {
                            Id = arrangement.Id,
                            Name = arrangement.Name,
                            MainImage = this.mapper.Map<ImageDto>(arrangement.ArrangementImages.Where(a => a.IsMainImage).FirstOrDefault().Image),
                            AgencyName = arrangement.Agency.Name,
                            MinPrice = arrangement.ArrangementPrices.Min(a => a.Price),
                            IsReserved = dbContext.Reservation.Any(r => r.ArrangmentId == arrangement.Id && r.ClientId == clientId && r.IsDeleted == false && (r.ReservationStatusId == (int)Core.Enum.ReservationStatus.Confirmed || r.ReservationStatusId == (int)Core.Enum.ReservationStatus.Pending))
                        });
                        if (arrangements.Count == 5)
                        {
                            break;
                        }
                    }
                }

                logger.LogInformation("GetRecommendedArrangementsByDestinationNames operation completed successfully with {Count} recommended arrangements.", arrangements.Count);
                return arrangements;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetRecommendedArrangementsByDestinationNames operation.");
                throw;
            }
        }

        public async Task<List<Destination>> GetAllDestinations()
        {
            logger.LogInformation("GetAllDestinations operation started.");

            try
            {
                var groupedByCity = await dbContext.Destination
                    .AsNoTracking()
                    .ToListAsync();

                var distinctDestinations = groupedByCity
                    .GroupBy(d => d.CityId)
                    .Select(g => g.First())
                    .ToList();

                logger.LogInformation("GetAllDestinations operation completed successfully with {Count} distinct destinations.", distinctDestinations.Count);
                return distinctDestinations;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetAllDestinations operation.");
                throw;
            }
        }

        public async Task<int> GetCount(Guid? agencyId = null)
        {
            logger.LogInformation("GetCount operation started for agencyId: {AgencyId}", agencyId);

            try
            {
                var count = await dbContext.Arrangement.Where(r => agencyId == null || r.AgencyId == agencyId).CountAsync();
                logger.LogInformation("GetCount operation completed successfully with count: {Count}", count);
                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetCount operation for agencyId: {AgencyId}", agencyId);
                throw;
            }
        }
    }
}
