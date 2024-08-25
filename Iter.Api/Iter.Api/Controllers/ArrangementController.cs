using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Model;
using Microsoft.AspNetCore.Authorization;
using Iter.Core.Enum;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.Admin) + "," + nameof(Roles.Coordinator) + "," + nameof(Roles.TouristGuide))]

    public class ArrangementController : BaseCRUDController<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel, ArrangementSearchResponse>
    {
        private readonly IArrangementService arrangementService;
        private readonly IRecommendationSystemService recommendationSystemService;
        private readonly ILogger<ArrangementController> logger;
        public ArrangementController(IArrangementService arrangementService, IRecommendationSystemService recommendationSystemService, ILogger<ArrangementController> logger) : base(arrangementService, logger)
        {
            this.arrangementService = arrangementService;
            this.recommendationSystemService = recommendationSystemService;
            this.logger = logger;
        }

        [HttpGet("arrangementPrice/{id}")]
        public virtual async Task<IActionResult> GetArrangementPrice(string id)
        {
            logger.LogInformation("GetArrangementPrice operation started for ID: {Id}", id);

            try
            {
                var price = await this.arrangementService.GetArrangementPriceAsync(new Guid(id));
                logger.LogInformation("GetArrangementPrice operation completed successfully for ID: {Id}", id);
                return Ok(price);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetArrangementPrice operation for ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("changeStatus/{id}")]
        public virtual async Task<IActionResult> ChangeStatus(string id, [FromBody] int status)
        {
            logger.LogInformation("ChangeStatus operation started for ID: {Id} with status: {Status}", id, status);

            try
            {
                await this.arrangementService.ChangeStatus(new Guid(id), status);
                logger.LogInformation("ChangeStatus operation completed successfully for ID: {Id} with status: {Status}", id, status);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during ChangeStatus operation for ID: {Id} with status: {Status}", id, status);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("recommendedArrangements/{id}")]
        public async Task<IActionResult> GetRecommendedArrangements(string id)
        {
            logger.LogInformation("GetRecommendedArrangements operation started for ID: {Id}", id);

            try
            {
                var recommendations = await this.recommendationSystemService.RecommendArrangement(new Guid(id));
                logger.LogInformation("GetRecommendedArrangements operation completed successfully for ID: {Id}", id);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetRecommendedArrangements operation for ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}