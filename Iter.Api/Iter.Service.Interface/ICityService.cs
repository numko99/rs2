using Iter.Services.Interfaces;
using Iter.Core.EntityModelss;
using Iter.Model;

namespace Iter.Services.Interface
{
    public interface ICityService : IBaseCrudService<City, CityUpsertRequest, CityUpsertRequest, CityResponse, CitySearchModel, CityResponse>
    {
        Task Update(int id, CityUpsertRequest request);

        Task<CityResponse> GetById(int id);

        Task Delete(int id);
    }
}