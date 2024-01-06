using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IAccommodationRepository : IBaseCrudRepository<Accommodation, AccommodationUpsertRequest, AccommodationUpsertRequest, AccommodationResponse, AgencySearchModel>
    {
    }
}