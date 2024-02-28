using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Services
{
    public class EmployeeArrangmentService : BaseCrudService<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, AgencySearchModel>, IEmployeeArrangmentService
    {
        public EmployeeArrangmentService(IEmployeeArrangmentRepository employeearrangmentRepository, IMapper mapper) : base(employeearrangmentRepository, mapper)
        {
        }
    }
}