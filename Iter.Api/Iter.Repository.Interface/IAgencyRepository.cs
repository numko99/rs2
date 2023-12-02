using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Repository.Interface
{
    public interface IAgencyRepository : IBaseCrudRepository<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse>
    {
        Task<List<AgencySearchResponse>> GetAgenciesSearch(int currentPage, int pageSize);
    }
}
