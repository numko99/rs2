using Iter.API.Controllers;
using Iter.Core;
using Iter.Core.Enum;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;
using Iter.Core.Requests;
using AutoMapper;
using Iter.Core.Search_Models;

namespace Iter.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Client) + "," + nameof(Roles.Admin) + "," + nameof(Roles.Coordinator))]
    public class CountryController : BaseCRUDController<Country, CountryUpsertRequest, CountryUpsertRequest, CountryResponse, CitySearchModel, CountryResponse>
    {
        private readonly ICountryService countryService;
        public CountryController(ICountryService countryService) : base(countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(string id)
        {
            if (int.TryParse(id, out int result))
            {
                var city = await this.countryService.GetById(result);
                return Ok(city);
            }
            else
            {
                return this.BadRequest();
            }
        }


        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(string id, [FromBody] CountryUpsertRequest request)
        {
            if (int.TryParse(id, out int result))
            {
                await this.countryService.Update(result, request);
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
                await this.countryService.Delete(result);
                return Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}