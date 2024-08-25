using AutoMapper;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class AccommodationService : BaseCrudService<Accommodation, AccommodationUpsertRequest, AccommodationUpsertRequest, AccommodationResponse, BaseSearchModel, AccommodationResponse>, IAccommodationService
    {
        public AccommodationService(IAccommodationRepository accommodationRepository, IMapper mapper, ILogger<AccommodationService> logger) : base(accommodationRepository, mapper, logger)
        {
        }
    }
}