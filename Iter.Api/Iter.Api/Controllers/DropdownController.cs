using AutoMapper;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide))]
    public class DropdownController : Controller
    {
        private readonly IDropdownRepository dropdownRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DropdownController> logger;

        public DropdownController(IDropdownRepository dropdownRepository, IMapper mapper, ILogger<DropdownController> logger)
        {
            this.dropdownRepository = dropdownRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int dropdownType, string? arrangementId = null, string? agencyId = null, string? countryId = null)
        {
            logger.LogInformation("Dropdown Get operation started with parameters: dropdownType={DropdownType}, arrangementId={ArrangementId}, agencyId={AgencyId}, countryId={CountryId}", dropdownType, arrangementId, agencyId, countryId);

            try
            {
                var list = await this.dropdownRepository.Get(dropdownType, arrangementId, agencyId, countryId);
                var mappedResult = this.mapper.Map<List<DropdownModel>>(list);

                logger.LogInformation("Dropdown Get operation completed successfully with {Count} results.", list.Count);

                return Ok(new PagedResult<DropdownModel>()
                {
                    Count = list.Count,
                    Result = mappedResult
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Dropdown Get operation with parameters: dropdownType={DropdownType}", dropdownType);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
