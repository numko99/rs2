using Iter.API.Controllers;
using Iter.Core.Enum;
using Iter.Core.EntityModels;
using Iter.Core.Responses;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iter.Core.Requests;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CityController : BaseCRUDController<City, CityUpsertRequest, CityUpsertRequest, CityResponse, CitySearchModel, CityResponse>
    {
        private readonly ICityService cityService;
        public CityController(ICityService cityService) : base(cityService)
        {
            this.cityService = cityService;
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(string id)
        {
            if (int.TryParse(id, out int result))
            {
                var city = await this.cityService.GetById(result);
                return Ok(city);
            }
            else
            {
                return this.BadRequest();
            }
        }


        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(string id, [FromBody] CityUpsertRequest request)
        {
            if (int.TryParse(id, out int result)){
                await this.cityService.Update(result, request);
                return Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(string id)
        {
            if (int.TryParse(id, out int result))
            {
                await this.cityService.Delete(result);
                return Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}