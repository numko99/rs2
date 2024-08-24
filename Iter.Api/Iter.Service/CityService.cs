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
using Iter.Core.Requests;
using Iter.Core.RequestParameterModels;
using Iter.Core.Models;
using Iter.Core.Search_Models;

namespace Iter.Services
{
    public class CityService : BaseCrudService<City, CityUpsertRequest, CityUpsertRequest, CityResponse, CitySearchModel, CityResponse>, ICityService
    {
        private readonly ICityRepository cityRepository;
        private readonly IMapper mapper;
        public CityService(ICityRepository cityRepository, IMapper mapper) : base(cityRepository, mapper)
        {
            this.cityRepository = cityRepository;
            this.mapper = mapper;
        }

        public virtual async Task<PagedResult<CityResponse>> Get(CitySearchModel searchObject)
        {
            int? countryId = null;

            if (int.TryParse(searchObject.CountryId, out var countryIdTemp))
            {
                countryId = countryIdTemp;
            }

            var searchData = await this.cityRepository.Get(searchObject.PageSize, searchObject.CurrentPage, searchObject.Name, countryId);
            return this.mapper.Map<PagedResult<CityResponse>>(searchData);
        }

        public virtual async Task<CityResponse> GetById(int id)
        {
            var entity = await this.cityRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid request");
            }

            return this.mapper.Map<CityResponse>(entity);
        }

        public async Task Update(int id, CityUpsertRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Invalid request");
            }

            var entity = await this.cityRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid id");
            }

            this.mapper.Map(request, entity);
            await this.cityRepository.UpdateAsync(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await this.cityRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid id");
            }

            await this.cityRepository.DeleteAsync(entity);
        }
    }
}