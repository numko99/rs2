using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core;
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
        public ArrangementController(IArrangementService arrangementService, IRecommendationSystemService recommendationSystemService) : base(arrangementService)
        {
            this.arrangementService = arrangementService;
            this.recommendationSystemService = recommendationSystemService;
        }

        [HttpGet("arrangementPrice/{id}")]
        public virtual async Task<IActionResult> GetArrangementPrice(string id)
        {
            return Ok(await this.arrangementService.GetArrangementPriceAsync(new Guid(id)));
        }

        [HttpPut("changeStatus/{id}")]
        public virtual async Task<IActionResult> ChangeStatus(string id, [FromBody] int status)
        {
            await this.arrangementService.ChangeStatus(new Guid(id), status);
            return Ok();
        }

        [HttpGet("recommendedArrangements/{id}")]
        public virtual async Task<IActionResult> GetRecommendedArrangements(string id)
        {
            return Ok(await this.recommendationSystemService.RecommendArrangement(new Guid(id)));
        }
    }
}