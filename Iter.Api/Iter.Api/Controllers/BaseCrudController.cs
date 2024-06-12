using Iter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Iter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseCRUDController<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> : BaseReadController<T, TGet, TSearchRequest, TSearchResponse> where TInsert : class where T : class where TGet : class where TSearchResponse : class
    {
        private readonly IBaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> baseCrudService;

        public BaseCRUDController(IBaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> baseCrudService) : base(baseCrudService)
        {
            this.baseCrudService = baseCrudService;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Insert([FromBody] TInsert request)
        {
            await this.baseCrudService.Insert(request);
            return Ok();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TUpdate request)
        {
            await this.baseCrudService.Update(new Guid(id), request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            await this.baseCrudService.Delete(new Guid(id));
            return Ok();
        }
    }
}