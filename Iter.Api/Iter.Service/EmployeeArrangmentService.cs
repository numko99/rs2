using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Microsoft.Extensions.Logging;

namespace Iter.Services
{
    public class EmployeeArrangmentService : BaseCrudService<EmployeeArrangment, EmployeeArrangmentUpsertRequest, EmployeeArrangmentUpsertRequest, EmployeeArrangmentResponse, EmployeeArrangementSearchModel, EmployeeArrangmentResponse>, IEmployeeArrangmentService
    {
        private readonly IEmployeeArrangmentRepository _employeeArrangmentRepository;
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly IMapper mapper;
        private readonly ILogger<EmployeeArrangmentService> logger;

        public EmployeeArrangmentService(IEmployeeArrangmentRepository employeearrangmentRepository, IMapper mapper, IEmployeeArrangmentRepository employeeArrangmentRepository, IUserAuthenticationService userAuthenticationService, ILogger<EmployeeArrangmentService> logger)
            : base(employeearrangmentRepository, mapper, logger)
        {
            _employeeArrangmentRepository = employeeArrangmentRepository;
            this.mapper = mapper;
            this.userAuthenticationService = userAuthenticationService;
            this.logger = logger;
        }

        public async Task<List<DropdownModel>> GetAvailableEmployeeArrangmentsAsync(Guid arrangementId, string dateFrom, string? dateTo)
        {
            logger.LogInformation("Getting available employee arrangements for Arrangement ID: {ArrangementId}, DateFrom: {DateFrom}, DateTo: {DateTo}", arrangementId, dateFrom, dateTo);

            try
            {
                var employees = await this._employeeArrangmentRepository.GetAvailableEmployeeArrangmentsAsync(arrangementId, DateTime.Parse(dateFrom), !string.IsNullOrEmpty(dateTo) ? DateTime.Parse(dateTo) : null);
                var result = this.mapper.Map<List<DropdownModel>>(employees);

                logger.LogInformation("Found {Count} available employees for Arrangement ID: {ArrangementId}", result.Count, arrangementId);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting available employee arrangements for Arrangement ID: {ArrangementId}", arrangementId);
                throw;
            }
        }

        public override async Task<PagedResult<EmployeeArrangmentResponse>> Get(EmployeeArrangementSearchModel searchObject)
        {
            logger.LogInformation("Getting employee arrangements with search parameters: {@SearchObject}", searchObject);

            try
            {
                if (searchObject.ArrangementId == null && searchObject.EmployeeId == null)
                {
                    var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();
                    searchObject.EmployeeId = currentUser.EmployeeId;
                }

                var data = await this._employeeArrangmentRepository.Get(this.mapper.Map<EmployeeArrangementRequestParameters>(searchObject));
                var result = this.mapper.Map<PagedResult<EmployeeArrangmentResponse>>(data);

                logger.LogInformation("Get operation completed successfully with {Count} results.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting employee arrangements with search parameters: {@SearchObject}", searchObject);
                throw;
            }
        }

        public override async Task Insert(EmployeeArrangmentUpsertRequest request)
        {
            logger.LogInformation("Inserting employee arrangements for Arrangement ID: {ArrangementId}", request.ArrangementId);

            try
            {
                var employes = new List<EmployeeArrangment>();
                foreach (var item in request.EmployeeIds)
                {
                    var employee = this.mapper.Map<EmployeeArrangment>(request);
                    employee.EmployeeId = new Guid(item);
                    employes.Add(employee);
                }

                await this._employeeArrangmentRepository.AddRangeAsync(employes, new Guid(request.ArrangementId));

                logger.LogInformation("Successfully inserted {Count} employee arrangements for Arrangement ID: {ArrangementId}", employes.Count, request.ArrangementId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while inserting employee arrangements for Arrangement ID: {ArrangementId}", request.ArrangementId);
                throw;
            }
        }
    }
}
