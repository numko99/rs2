using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;

namespace Iter.Services.Interface
{
    public interface ICountryService : IBaseCrudService<Country, CountryUpsertRequest, CountryUpsertRequest, CountryResponse, CitySearchModel, CountryResponse>
    {
        Task Update(int id, CountryUpsertRequest request);

        Task<CountryResponse> GetById(int id);

        Task Delete(int id);
    }
}