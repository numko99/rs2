using Iter.Core.Search_Models;
using Iter.Services;
using Iter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iter.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseReadController<T, TGet, TSearch> : ControllerBase where T : class where TGet : class
    {
        protected readonly IBaseReadService<T, TGet, TSearch> baseReadService;
        public BaseReadController(IBaseReadService<T, TGet, TSearch> baseReadService)
        {
            this.baseReadService = baseReadService;
        }

        [HttpGet("getAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await this.baseReadService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TSearch searchModel)
        {
            var agencies = await baseReadService.Get(searchModel);

            return Ok(agencies);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(string id)
        {
            return Ok(await this.baseReadService.GetById(new Guid(id)));
        }

    }
}