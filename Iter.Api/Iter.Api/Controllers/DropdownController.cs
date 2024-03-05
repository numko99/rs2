using Iter.Core.Enum;
using Iter.Repository;
using Iter.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : Controller
    {
        private readonly IDropdownRepository dropdownRepository;

        public DropdownController(IDropdownRepository dropdownRepository)
        {
            this.dropdownRepository = dropdownRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int dropdownType, string? arrangementId = null, string? agencyId = null)
        {
            return Ok(await this.dropdownRepository.Get(dropdownType, arrangementId, agencyId));
        }
    }
}
