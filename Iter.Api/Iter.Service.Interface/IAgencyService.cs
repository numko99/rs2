using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Services.Interface
{
    public interface IAgencyService : IBaseCrudService<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse>
    {
        Task<List<AgencySearchResponse>> GetAgenciesSearch(int currentPage, int pageSize);
    }
}
