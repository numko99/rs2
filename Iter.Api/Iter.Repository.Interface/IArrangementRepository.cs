using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IArrangementRepository : IBaseCrudRepository<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel, ArrangementSearchResponse>
    {
        Task<ArrangementPrice> GetArrangementPriceAsync(Guid id);

        Task SmartUpdateAsync(Arrangement arrangement);
    }
}