using Iter.Core.EntityModels;
using Iter.Core.Search_Models;
using Iter.Core.Models;
using Iter.Core.Dto;
using Iter.Core.RequestParameterModels;

namespace Iter.Repository.Interface
{
    public interface IArrangementRepository : IBaseCrudRepository<Arrangement>
    {
        Task<ArrangementPrice> GetArrangementPriceAsync(Guid id);

        Task SmartUpdateAsync(Arrangement arrangement);

        Task<List<ArrangementSearchDto>> GetRecommendedArrangementsByDestinationNames(List<int> cities, Guid? clientId);

        Task<List<Destination>> GetAllDestinations();

        Task<PagedResult<ArrangementSearchDto>> Get(ArrangmentSearchParameters? search);

        Task<int> GetCount(Guid? agencyId = null);
    }
}