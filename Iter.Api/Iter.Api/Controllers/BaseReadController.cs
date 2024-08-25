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
        private readonly ILogger<BaseReadController<T, TGet, TSearchRequest, TSearchResponse>> logger;

        public BaseReadController(IBaseReadService<T, TGet, TSearchRequest, TSearchResponse> baseReadService, ILogger<BaseReadController<T, TGet, TSearchRequest, TSearchResponse>> logger)
        {
            this.baseReadService = baseReadService;
            this.logger = logger;
        }

        [HttpGet("getAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll operation started.");

            try
            {
                var result = await this.baseReadService.GetAll();
                logger.LogInformation("GetAll operation completed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetAll operation.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TSearchRequest searchModel)
        {
            logger.LogInformation("Get operation started with search model: {@SearchModel}", searchModel);

            try
            {
                var result = await baseReadService.Get(searchModel);
                logger.LogInformation("Get operation completed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during Get operation with search model: {@SearchModel}", searchModel);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(string id)
        {
            logger.LogInformation("GetById operation started for ID: {Id}", id);

            try
            {
                var result = await this.baseReadService.GetById(new Guid(id));
                logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetById operation for ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}