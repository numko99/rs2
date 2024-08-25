using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class DestinationService : BaseCrudService<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel, DestinationResponse>, IDestinationService
    {
        public DestinationService(IDestinationRepository destinationRepository, IMapper mapper, ILogger<DestinationService> logger) : base(destinationRepository, mapper, logger)
        {
        }
    }
}