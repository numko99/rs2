using Iter.API.Controllers;
using Iter.Core.EntityModels;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Search_Models;
using Iter.Core;

namespace Iter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrangementController : BaseCRUDController<Arrangement, ArrangementUpsertRequest, ArrangementUpsertRequest, ArrangementResponse, ArrangmentSearchModel>
    {
        private readonly IArrangementService arrangementService;
        public ArrangementController(IArrangementService arrangementService) : base(arrangementService)
        {
            this.arrangementService = arrangementService;
        }

        [HttpGet("arrangementPrice/{id}")]
        public virtual async Task<IActionResult> GetArrangementPrice(string id)
        {
            return Ok(await this.arrangementService.GetArrangementPriceAsync(new Guid(id)));
        }
    }
}