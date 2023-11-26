using Iter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BaseCRUDController<T, TInsert, TUpdate, TGet> : BaseReadController<T, TGet> where TInsert : class where T : class where TGet : class
    {
        private readonly IBaseCrudService<T, TInsert, TUpdate, TGet> baseCrudService;

        public BaseCRUDController(IBaseCrudService<T, TInsert, TUpdate, TGet> baseCrudService) : base(baseCrudService)
        {
            this.baseCrudService = baseCrudService;
        }

        [HttpPost("insert")]
        public virtual async Task<IActionResult> Insert([FromBody] TInsert request)
        {
            await this.baseCrudService.Insert(request);
            return Ok();
        }

        [HttpPut("update")]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] TUpdate request)
        {
            await this.baseCrudService.Update(id, request);
            return Ok();
        }

        [HttpDelete("delete")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            await this.baseCrudService.Delete(id);
            return Ok();
        }
    }
}