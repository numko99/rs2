using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;

namespace Iter.Services
{
    public class CountryService : BaseCrudService<Country, CountryUpsertRequest, CountryUpsertRequest, CountryResponse, CitySearchModel, CountryResponse>, ICountryService
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;
        public CountryService(ICountryRepository countryRepository, IMapper mapper) : base(countryRepository, mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        public virtual async Task<PagedResult<CountryResponse>> Get(CitySearchModel searchObject)
        {
            int? countryId = null;

            if (int.TryParse(searchObject.CountryId, out var countryIdTemp))
            {
                countryId = countryIdTemp;
            }

            var searchData = await this.countryRepository.Get(searchObject.PageSize, searchObject.CurrentPage, searchObject.Name);
            return this.mapper.Map<PagedResult<CountryResponse>>(searchData);
        }

        public virtual async Task<CountryResponse> GetById(int id)
        {
            var entity = await this.countryRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid request");
            }

            return this.mapper.Map<CountryResponse>(entity);
        }

        public async Task Update(int id, CountryUpsertRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Invalid request");
            }

            var entity = await this.countryRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid id");
            }

            this.mapper.Map(request, entity);
            await this.countryRepository.UpdateAsync(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await this.countryRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid id");
            }

            await this.countryRepository.DeleteAsync(entity);
        }
    }
}