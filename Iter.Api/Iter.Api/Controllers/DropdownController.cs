using AutoMapper;
using Iter.Core.Enum;
using Iter.Core.Models;
using Iter.Repository;
using Iter.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide))]
    public class DropdownController : Controller
    {
        private readonly IDropdownRepository dropdownRepository;
        private readonly IMapper mapper;

        public DropdownController(IDropdownRepository dropdownRepository, IMapper mapper)
        {
            this.dropdownRepository = dropdownRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int dropdownType, string? arrangementId = null, string? agencyId = null, string? countryId = null)
        {
            var list = await this.dropdownRepository.Get(dropdownType, arrangementId, agencyId, countryId);
            return Ok(new PagedResult<DropdownModel>()
            {
                Count = list.Count,
                Result = this.mapper.Map<List<DropdownModel>>(list)
            });
        }
    }
}
