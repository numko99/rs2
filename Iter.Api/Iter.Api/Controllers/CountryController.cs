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
    public class CountryController : BaseCRUDController<Country, CountryUpsertRequest, CountryUpsertRequest, CountryResponse, CitySearchModel, CountryResponse>
    {
        private readonly ICountryService countryService;
        private readonly ILogger<CountryController> logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger) : base(countryService, logger)
        {
            this.countryService = countryService;
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
                    var country = await this.countryService.GetById(result);
                    logger.LogInformation("GetById operation completed successfully for ID: {Id}", id);
                    return Ok(country);
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
        public override async Task<IActionResult> Update(string id, [FromBody] CountryUpsertRequest request)
        {
            logger.LogInformation("Update operation started for ID: {Id} with request data: {@Request}", id, request);

            if (int.TryParse(id, out int result))
            {
                try
                {
                    await this.countryService.Update(result, request);
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
                    await this.countryService.Delete(result);
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
