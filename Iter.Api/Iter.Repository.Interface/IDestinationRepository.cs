using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IDestinationRepository : IBaseCrudRepository<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel>
    {
    }
}