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
using Iter.Core.Models;

namespace Iter.Services
{
    public class EmployeeArrangmentService : BaseCrudService<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>, IEmployeeArrangmentService
    {
        private readonly IEmployeeArrangmentRepository _employeeArrangmentRepository;
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly IMapper mapper;
        public EmployeeArrangmentService(IEmployeeArrangmentRepository employeearrangmentRepository, IMapper mapper, IEmployeeArrangmentRepository employeeArrangmentRepository, IUserAuthenticationService userAuthenticationService) : base(employeearrangmentRepository, mapper)
        {
            _employeeArrangmentRepository = employeeArrangmentRepository;
            this.mapper = mapper;
            this.userAuthenticationService = userAuthenticationService;
        }

        public async Task<List<DropdownModel>> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, string dateFrom, string? dateTo)
        {
            var employees = await this._employeeArrangmentRepository.GetAvailableEmployeeArrangmentsAsync(arrangementId, DateTime.Parse(dateFrom), !string.IsNullOrEmpty(dateTo) ? DateTime.Parse(dateTo) : null);
            return this.mapper.Map<List<DropdownModel>>(employees);
        }

        public override async Task<PagedResult<EmployeeArrangmentResponse>> Get(EmployeeArrangementSearchModel searchObject)
        {
            if (searchObject.ArrangementId == null && searchObject.EmployeeId == null)
            {
                var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
                searchObject.EmployeeId = currentUser.EmployeeId;
            }

            return await this.baseReadRepository.Get(searchObject);
        }

        public override async Task Insert(EmployeeArrangmentUpsertRequest request)
        {
            var employes = new List<EmployeeArrangment>();
            foreach (var item in request.EmployeeIds)
            {
                var employee = this.mapper.Map<EmployeeArrangment>(request);
                employee.EmployeeId = new Guid(item);
                employes.Add(employee);
            }

            await this._employeeArrangmentRepository.AddRangeAsync(employes, new Guid(request.ArrangementId));
        }
    }
}