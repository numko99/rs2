using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Iter.Repository
{
    public class ArrangementRepository : BaseCrudRepository<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel>, IArrangementRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;

        public ArrangementRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async override Task<PagedResult<ArrangementResponse>> Get(ArrangmentSearchModel? search)
        {
            var query = dbContext.Set<Arrangement>().AsQueryable();

            PagedResult<ArrangementResponse> result = new PagedResult<ArrangementResponse>();

            if (!string.IsNullOrEmpty(search?.Name))
            {
                query = query.Where(a => a.Name.Contains(search.Name));
            }

            if (!string.IsNullOrEmpty(search?.AgencyId))
            {
                var guid = new Guid(search.AgencyId);
                query = query.Where(a => a.AgencyId == guid);
            }
            if (search?.DateFrom != null)
            {
                var dateFrom = DateTime.ParseExact(search.DateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                query = query.Where(a => a.StartDate >= dateFrom);
            }

            if (search?.DateTo != null)
            {
                var dateTo = DateTime.ParseExact(search.DateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                query = query.Where(a => a.EndDate < dateTo || (a.EndDate == null && dateTo > a.StartDate ));
            }

            query = query.Where(q => q.IsDeleted == false);


            query = query.Include(nameof(Agency)).Include(nameof(ArrangementStatus));

            result.Count = await query.CountAsync();

            if (search?.CurrentPage.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Skip((search.CurrentPage.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            }
            var list = await query.OrderByDescending(q => q.CreatedAt).ToListAsync();
            var tmp = mapper.Map<List<ArrangementResponse>>(list);
            result.Result = tmp;
            return result;
        }

        public override async Task<Arrangement?> GetById(Guid id)
        {
            var arrangement = await dbContext.Arrangement
                    .Include(a => a.Agency)
                    .Include(a => a.ArrangementStatus)
                    .Include(a => a.ArrangementPrices)
                    .Include(a => a.ArrangementImages)
                        .ThenInclude(ai => ai.Image)
                    .Include(a => a.Destinations)
                        .ThenInclude(ai => ai.Accommodation)
                        .ThenInclude(ai => ai.HotelAddress)
                    .Where(a => a.Id == id && a.IsDeleted == false)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            return arrangement;
        }
        public async Task SmartUpdateAsync(Arrangement entity)
        {
            var pricesIds = entity.ArrangementPrices.Select(a => a.Id).ToList();
            var pricesToDelete = dbContext.ArrangementPrice
               .Where(ap => ap.ArrangementId == entity.Id && !pricesIds.Contains(ap.Id))
               .ToList();

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

            dbContext.ArrangementPrice.RemoveRange(pricesToDelete);
            dbContext.ArrangementImage.RemoveRange(existingImages.ToList());
            dbContext.Image.RemoveRange(imagesToDelete.ToList());
            dbContext.Destination.RemoveRange(destinationsToDelete.ToList());
            await dbContext.SaveChangesAsync();

            foreach (var destination in entity.Destinations)
            {
                var existingDestination = dbContext.Destination.AsNoTracking().FirstOrDefault(d => d.Id == destination.Id);
                if (existingDestination != null)
                {
                    dbContext.Entry(existingDestination).CurrentValues.SetValues(destination);
                }
                else
                {
                    dbContext.Destination.Add(destination);
                }
            }

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
            dbContext.ArrangementImage.AddRange(entity.ArrangementImages);

            dbContext.Arrangement.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async override Task DeleteAsync(Arrangement entity)
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
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<ArrangementPrice> GetArrangementPriceAsync(Guid id)
        {
            var arrangementPrice = await this.dbContext.ArrangementPrice.Where(a => a.Id == id).FirstOrDefaultAsync();

            return arrangementPrice;
        }

        private void AddPricesAndImages(Arrangement entity)
        {
            foreach (var price in entity.ArrangementPrices)
            {
                price.ArrangementId = entity.Id;
                dbContext.ArrangementPrice.Add(price);
            }

            foreach (var image in entity.ArrangementImages)
            {
                image.ArrangementId = entity.Id;
                dbContext.ArrangementImage.Add(image);
            }
        }

        private async Task DeletePricesAndImages(Arrangement entity)
        {
            var existingPrices = await dbContext.ArrangementPrice
                .Where(ap => ap.ArrangementId == entity.Id)
                .ToListAsync();
            dbContext.ArrangementPrice.RemoveRange(existingPrices);

            var existingImages = await dbContext.ArrangementImage
                .Where(ai => ai.ArrangementId == entity.Id)
                .ToListAsync();
            var imageIds = existingImages.Select(ai => ai.ImageId).ToList();
            var imagesToDelete = await dbContext.Image
                .Where(i => imageIds.Contains(i.Id))
                .ToListAsync();
            dbContext.ArrangementImage.RemoveRange(existingImages);
            dbContext.Image.RemoveRange(imagesToDelete);
        }
    }
}