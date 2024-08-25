using Iter.Model;
using Iter.Services.Interfaces;
using Iter.Core.EntityModelss;

namespace Iter.Services.Interface
{
    public interface ICountryService : IBaseCrudService<Country, CountryUpsertRequest, CountryUpsertRequest, CountryResponse, CitySearchModel, CountryResponse>
    {
        Task Update(int id, CountryUpsertRequest request);

        Task<CountryResponse> GetById(int id);

        Task Delete(int id);
    }
}