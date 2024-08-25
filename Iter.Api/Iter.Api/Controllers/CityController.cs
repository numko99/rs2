using Iter.API.Controllers;
using Iter.Core.Enum;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.EntityModelss;
using Iter.Model;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CityController : BaseCRUDController<City, CityUpsertRequest, CityUpsertRequest, CityResponse, CitySearchModel, CityResponse>
    {
        private readonly ICityService cityService;
        private readonly ILogger<CityController> logger;

        public CityController(ICityService cityService, ILogger<CityController> logger) : base(cityService, logger)
        {
            this.cityService = cityService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(string id)
        {
            logger.LogInformation("GetById operation started for ID: {Id}", id);

            if (int.TryParse(id, out int result))
            {
                try
                {
                    var city = await this.cityService.GetById(result);
                    logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);
                    return Ok(city);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during GetById operation for ID: {Id}", id);
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
            else
            {
                logger.LogWarning("Invalid ID format for GetById operation: {Id}", id);
                return this.BadRequest("Invalid ID format.");
            }
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(string id, [FromBody] CityUpsertRequest request)
        {
            logger.LogInformation("Update operation started for ID: {Id} with request data: {@Request}", id, request);

            if (int.TryParse(id, out int result))
            {
                try
                {
                    await this.cityService.Update(result, request);
                    logger.LogInformation("Update operation completed successfully for ID: {Id}", id);
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during Update operation for ID: {Id}", id);
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
            else
            {
                logger.LogWarning("Invalid ID format for Update operation: {Id}", id);
                return this.BadRequest("Invalid ID format.");
            }
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(string id)
        {
            logger.LogInformation("Delete operation started for ID: {Id}", id);

            if (int.TryParse(id, out int result))
            {
                try
                {
                    await this.cityService.Delete(result);
                    logger.LogInformation("Delete operation completed successfully for ID: {Id}", id);
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during Delete operation for ID: {Id}", id);
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
            else
            {
                logger.LogWarning("Invalid ID format for Delete operation: {Id}", id);
                return this.BadRequest("Invalid ID format.");
            }
        }
    }
}
