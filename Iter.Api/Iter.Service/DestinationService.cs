using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;

namespace Iter.Services
{
    public class DestinationService : BaseCrudService<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel>, IDestinationService
    {
        public DestinationService(IDestinationRepository destinationRepository, IMapper mapper) : base(destinationRepository, mapper)
        {
        }
    }
}