using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IAccommodationService : IBaseCrudService<Accommodation, AccommodationUpsertRequest, AccommodationUpsertRequest, AccommodationResponse, BaseSearchModel, AccommodationResponse>
    {
    }
}