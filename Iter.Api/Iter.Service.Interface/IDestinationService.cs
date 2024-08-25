using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IDestinationService : IBaseCrudService<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel, DestinationResponse>
    {
    }
}