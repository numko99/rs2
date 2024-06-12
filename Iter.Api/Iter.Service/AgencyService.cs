using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;

namespace Iter.Services
{
    public class AgencyService: BaseCrudService<Agency, AgencyInsertRequest, AgencyInsertRequest, AgencyResponse, AgencySearchModel, AgencyResponse>, IAgencyService
    {
        private readonly IAgencyRepository agencyRepository;
        private readonly IMapper mapper;
        public AgencyService(IAgencyRepository agencyRepository, IMapper mapper): base(agencyRepository, mapper)
        {
            this.agencyRepository = agencyRepository;
            this.mapper = mapper;
        }

        public override async Task Update(Guid Id, AgencyInsertRequest request)
        {
            var agency = await agencyRepository.GetById(Id);
            this.mapper.Map(request, agency);

            if (request.Logo == null)
            {
                agency.ImageId = null;
            }

            await this.agencyRepository.UpdateAsync(agency);
        }

        public async Task<AgencyResponse?> GetByEmployeeId(Guid employeeId)
        {
            var agency = await this.agencyRepository.GetByEmployeeId(employeeId);
            return this.mapper.Map<AgencyResponse>(agency);
        }
    }
}
