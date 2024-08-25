using AutoMapper;
using Iter.Model;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class ArrangementService : BaseCrudService<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel, ArrangementSearchResponse>, IArrangementService
    {
        private readonly IMapper _mapper;
        private readonly IArrangementRepository _arrangementRepository;
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly ILogger<ArrangementService> logger;

        public ArrangementService(IArrangementRepository arrangementRepository, IMapper mapper, IUserAuthenticationService userAuthenticationService, ILogger<ArrangementService> logger) : base(arrangementRepository, mapper, logger)
        {
            _mapper = mapper;
            _arrangementRepository = arrangementRepository;
            this.userAuthenticationService = userAuthenticationService;
            this.logger = logger;
        }

        public async Task ChangeStatus(Guid id, int status)
        {
            logger.LogInformation("ChangeStatus operation started for Arrangement ID: {Id} with status: {Status}", id, status);

            try
            {
                var arrangement = await _arrangementRepository.GetById(id);

                if (arrangement == null)
                {
                    logger.LogWarning("Arrangement with ID {Id} not found.", id);
                    return;
                }

                arrangement.ArrangementStatusId = status;
                await _arrangementRepository.UpdateAsync(arrangement);

                logger.LogInformation("ChangeStatus operation completed successfully for Arrangement ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during ChangeStatus operation for Arrangement ID: {Id}", id);
                throw;
            }
        }

        public override async Task<PagedResult<ArrangementSearchResponse>> Get(ArrangmentSearchModel searchObject)
        {
            logger.LogInformation("Get operation started with search parameters: {@SearchObject}", searchObject);

            try
            {
                var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
                searchObject.CurrentUserId = currentUser.ClientId;
                var arrangementSearchData = await this._arrangementRepository.Get(this._mapper.Map<ArrangmentSearchParameters>(searchObject));

                var result = this._mapper.Map<PagedResult<ArrangementSearchResponse>>(arrangementSearchData);

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search parameters: {@SearchObject}", searchObject);
                throw;
            }
        }

        public async Task<ArrangementPriceResponse> GetArrangementPriceAsync(Guid id)
        {
            logger.LogInformation("GetArrangementPriceAsync operation started for Arrangement ID: {Id}", id);

            try
            {
                var arrangementPrices = await _arrangementRepository.GetArrangementPriceAsync(id);

                if (arrangementPrices == null)
                {
                    logger.LogWarning("No prices found for Arrangement ID: {Id}", id);
                    return null;
                }

                var result = this._mapper.Map<ArrangementPriceResponse>(arrangementPrices);

                logger.LogInformation("GetArrangementPriceAsync operation completed successfully for Arrangement ID: {Id}", id);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetArrangementPriceAsync operation for Arrangement ID: {Id}", id);
                throw;
            }
        }

        public override async Task Insert(ArrangementUpsertRequest request)
        {
            logger.LogInformation("Insert operation started.");

            try
            {
                var arrangement = this._mapper.Map<Arrangement>(request);

                ProcesMainImageAndPrices(request, arrangement);
                await this._arrangementRepository.AddAsync(arrangement);

                logger.LogInformation("Insert operation completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Insert operation.");
                throw;
            }
        }

        public override async Task Update(Guid id, ArrangementUpsertRequest request)
        {
            logger.LogInformation("Update operation started for Arrangement ID: {Id}", id);

            try
            {
                if (request == null)
                {
                    logger.LogWarning("Invalid request for Update operation.");
                    throw new ArgumentException("Invalid request");
                }

                var arrangement = await this._arrangementRepository.GetById(id);

                if (arrangement == null)
                {
                    logger.LogWarning("Arrangement with ID {Id} not found.", id);
                    throw new ArgumentException("Invalid id");
                }

                var newDestinations = request.Destinations
                    .Where(reqDest => reqDest.Id == null)
                    .Select(reqDest => _mapper.Map<Destination>(reqDest))
                    .ToList();

                arrangement.Destinations.AddRange(newDestinations);

                foreach (var destination in arrangement.Destinations.ToList())
                {
                    var requestDestination = request.Destinations.FirstOrDefault(reqDest => reqDest.Id != null && new Guid(reqDest.Id) == destination.Id);
                    if (requestDestination != null)
                    {
                        _mapper.Map(requestDestination, destination);
                    }
                    else if (requestDestination == null && destination.Id != Guid.Empty)
                    {
                        arrangement.Destinations.Remove(destination);
                    }
                }

                this._mapper.Map(request, arrangement);
                ProcesMainImageAndPrices(request, arrangement);

                await this._arrangementRepository.SmartUpdateAsync(arrangement);

                logger.LogInformation("Update operation completed successfully for Arrangement ID: {Id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Update operation for Arrangement ID: {Id}", id);
                throw;
            }
        }

        private void ProcesMainImageAndPrices(ArrangementUpsertRequest request, Arrangement arrangement)
        {
            logger.LogInformation("ProcesMainImageAndPrices operation started.");

            var mainImage = this._mapper.Map<ArrangementImage>(request.MainImage);
            mainImage.IsMainImage = true;
            arrangement.ArrangementImages.Add(mainImage);

            if (request.Price != null)
            {
                arrangement.ArrangementPrices.Add(new ArrangementPrice()
                {
                    Price = request.Price.Value,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }

            logger.LogInformation("ProcesMainImageAndPrices operation completed successfully.");
        }
    }
}
