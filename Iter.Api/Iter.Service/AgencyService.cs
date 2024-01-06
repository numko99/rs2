using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;

namespace Iter.Services
{
    public class AgencyService: BaseCrudService<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel>, IAgencyService
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
            entity.Address = this.mapper.Map<Address>(request);
            await this.agencyRepository.AddAsync(entity);
        }

        public override async Task Update(Guid Id, AgencyInsertRequest request)
        {
            var agency = await agencyRepository.GetById(Id);
            this.mapper.Map(request, agency);
            this.mapper.Map(request, agency.Address);

            await this.agencyRepository.UpdateAsync(agency);
        }

        public override async Task<PagedResult<AgencyResponse>> Get(AgencySearchModel agencySearch)
        {
            return await this.agencyRepository.Get(agencySearch);
        }
    }
}
