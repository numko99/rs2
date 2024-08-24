using Iter.Core.EntityModels;
using Iter.Core.Responses;
using Iter.Services.Interfaces;
using Iter.Core.Requests;
using Iter.Core.RequestParameterModels;
using Iter.Core.Search_Models;

namespace Iter.Services.Interface
{
    public interface ICityService : IBaseCrudService<City, CityUpsertRequest, CityUpsertRequest, CityResponse, CitySearchModel, CityResponse>
    {
        Task Update(int id, CityUpsertRequest request);

        Task<CityResponse> GetById(int id);

        Task Delete(int id);
    }
}