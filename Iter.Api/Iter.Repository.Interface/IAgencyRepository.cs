using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IAgencyRepository : IBaseCrudRepository<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel>
    {
    }
}
