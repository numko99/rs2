using Iter.Core.Search_Models;
using Iter.Services;
using Iter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iter.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseReadController<T, TGet, TSearchRequest, TSearchResponse> : ControllerBase where T : class where TGet : class where TSearchResponse : class
    {
        protected readonly IBaseReadService<T, TGet, TSearchRequest, TSearchResponse> baseReadService;
        public BaseReadController(IBaseReadService<T, TGet, TSearchRequest, TSearchResponse> baseReadService)
        {
            this.baseReadService = baseReadService;
        }

        [HttpGet("getAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await this.baseReadService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TSearchRequest searchModel)
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