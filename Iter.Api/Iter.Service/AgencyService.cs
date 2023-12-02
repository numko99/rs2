using AutoMapper;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Services
{
    public class AgencyService: BaseCrudService<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse>, IAgencyService
    {
        private readonly IAgencyRepository agencyRepository;
        private readonly IMapper mapper;
        public AgencyService(IAgencyRepository agencyRepository, IMapper mapper): base(agencyRepository, mapper)
        {
            this.agencyRepository = agencyRepository;
            this.mapper = mapper;
        }

        public override async Task Insert(AgencyInsertRequest request)
        {
            var entity = this.mapper.Map<Agency>(request);
            await this.agencyRepository.AddAsync(entity);
        }

        public override async Task<AgencyResponse> GetById(Guid id)
        {
            var entity = await this.agencyRepository.GetById(id);
            if (entity == null)
            {
                throw new ArgumentException("Invalid request");
            }
            return this.mapper.Map<AgencyResponse>(entity);
        }

        public override async Task Delete(Guid id)
        {
            var entity = await this.agencyRepository.GetById(id);

            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }

            await this.agencyRepository.DeleteAsync(entity);
        }

        public async Task<List<AgencySearchResponse>> GetAgenciesSearch(int currentPage, int pageSize)
        {
            return await this.agencyRepository.GetAgenciesSearch(currentPage, pageSize);
        }
    }
}
