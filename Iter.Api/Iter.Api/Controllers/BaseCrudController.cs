using Iter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Iter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseCRUDController<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> : BaseReadController<T, TGet, TSearchRequest, TSearchResponse> where TInsert : class where T : class where TGet : class where TSearchResponse : class
    {
        private readonly IBaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> baseCrudService;
        private readonly ILogger<BaseCRUDController<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse>> logger;
        public BaseCRUDController(IBaseCrudService<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse> baseCrudService, ILogger<BaseCRUDController<T, TInsert, TUpdate, TGet, TSearchRequest, TSearchResponse>> logger) : base(baseCrudService, logger)
        {
            this.baseCrudService = baseCrudService;
            this.logger = logger;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Insert([FromBody] TInsert request)
        {
            logger.LogInformation("Insert operation started with request data: {@Request}", request);

            try
            {
                await this.baseCrudService.Insert(request);
                logger.LogInformation("Insert operation completed successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Insert operation.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TUpdate request)
        {
           logger.LogInformation("Update operation started for ID: {Id}", id);
           logger.LogDebug("Update request data: {@Request}", request);

            try
            {
                await this.baseCrudService.Update(new Guid(id), request);
                logger.LogInformation("Update operation completed successfully for ID: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Update operation for ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            logger.LogInformation("Delete operation started for ID: {Id}", id);

            try
            {
                await this.baseCrudService.Delete(new Guid(id));
                logger.LogInformation("Delete operation completed successfully for ID: {Id}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Delete operation for ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}