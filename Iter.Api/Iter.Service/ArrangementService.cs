using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;

namespace Iter.Services
{
    public class ArrangementService : BaseCrudService<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel>, IArrangementService
    {
        public ArrangementService(IArrangementRepository arrangementRepository, IMapper mapper) : base(arrangementRepository, mapper)
        {
        }
    }
}