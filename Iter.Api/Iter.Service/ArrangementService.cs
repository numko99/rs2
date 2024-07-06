using AutoMapper;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;

namespace Iter.Services
{
    public class ArrangementService : BaseCrudService<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel, ArrangementSearchResponse>, IArrangementService
    {
        private readonly IMapper _mapper;
        private readonly IArrangementRepository _arrangementRepository;
        private readonly IUserAuthenticationService userAuthenticationService;
        public ArrangementService(IArrangementRepository arrangementRepository, IMapper mapper, IUserAuthenticationService userAuthenticationService) : base(arrangementRepository, mapper)
        {
            _mapper = mapper;
            _arrangementRepository = arrangementRepository;
            this.userAuthenticationService = userAuthenticationService;
        }

        public async Task ChangeStatus(Guid id, int status)
        {
            var arrangement = await  _arrangementRepository.GetById(id);

            if (arrangement == null)
            {
                return;
            }

            arrangement.ArrangementStatusId = status;
            await _arrangementRepository.UpdateAsync(arrangement);
        }

        public override async Task<PagedResult<ArrangementSearchResponse>> Get(ArrangmentSearchModel searchObject)
        {
            var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
            searchObject.CurrentUserId = currentUser.ClientId;
            var arrangementSearchData = await this._arrangementRepository.Get(this._mapper.Map<ArrangmentSearchParameters>(searchObject));

            return this._mapper.Map<PagedResult<ArrangementSearchResponse>>(arrangementSearchData);
        }

        public async Task<ArrangementPriceResponse> GetArrangementPriceAsync(Guid id)
        {
            var arrangementPrices = await _arrangementRepository.GetArrangementPriceAsync(id);

            return this._mapper.Map<ArrangementPriceResponse>(arrangementPrices);
        }

        public override async Task Insert(ArrangementUpsertRequest request)
        {
            try
            {
                var arrangement = this._mapper.Map<Arrangement>(request);

                ProcesMainImageAndPrices(request, arrangement);
                await this._arrangementRepository.AddAsync(arrangement);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override async Task Update(Guid id, ArrangementUpsertRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Invalid request");
            }

            var arrangement = await this._arrangementRepository.GetById(id);

            if (arrangement == null)
            {
                throw new ArgumentException("Invalid id");
            }
            var newDestinations = request.Destinations
                //.Where(reqDest => !arrangement.Destinations.Any(arrDest => arrDest.Id == new Guid(reqDest.Id)))
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
        }

        private void ProcesMainImageAndPrices(ArrangementUpsertRequest request, Arrangement arrangement)
        {
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
        }
    }
}