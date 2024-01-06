using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;

namespace Iter.Repository
{
    public class EmployeeArrangmentRepository : BaseCrudRepository<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, AgencySearchModel>, IEmployeeArrangmentRepository
    {
        public EmployeeArrangmentRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}