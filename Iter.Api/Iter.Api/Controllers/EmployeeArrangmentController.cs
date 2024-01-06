using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeArrangmentController : BaseCRUDController<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, AgencySearchModel>
    {
        public EmployeeArrangmentController(IEmployeeArrangmentService employeearrangmentService) : base(employeearrangmentService)
        {
        }
    }
}