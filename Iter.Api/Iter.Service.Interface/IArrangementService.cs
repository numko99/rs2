using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IArrangementService : IBaseCrudService<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel>
    {
    }
}