using Iter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseReadController<T, TGet> : ControllerBase where T : class where TGet : class
    {
        protected readonly IBaseReadService<T, TGet> baseReadService;
        public BaseReadController(IBaseReadService<T, TGet> baseReadService)
        {
            this.baseReadService = baseReadService;
        }

        [HttpGet("getAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await this.baseReadService.GetAll());
        }

        [HttpGet("details")]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await this.baseReadService.GetById(id));
        }

    }
}