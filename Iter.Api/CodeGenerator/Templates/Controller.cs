public class ControllerTemplate
{
    private string entityName;
    string controllerTemplate;
    public string Generate()
    {
        return controllerTemplate.Replace("{entityName}", entityName);
    }

    public ControllerTemplate(string entityName)
    {
        this.entityName = entityName;
        this.controllerTemplate = 
$@"using Iter.API.Controllers;
using Iter.Model;
using Iter.Core.Enum;
using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Responses;
using Iter.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Iter.Api.Controllers
{{
    [Route(""[controller]"")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Client) + "","" + nameof(Roles.Admin) + "","" + nameof(Roles.Coordinator))]
    public class {entityName}Controller : BaseCRUDController<{entityName}, {entityName}UpsertRequest, {entityName}UpsertRequest, {entityName}Response, {entityName}SearchModel, {entityName}Response>
    {{
        private readonly I{entityName}Service {entityName.ToLower()}Service;
        public {entityName}Controller(I{entityName}Service {entityName.ToLower()}Service) : base({entityName.ToLower()}Service)
        {{
            this.{entityName.ToLower()}Service = {entityName.ToLower()}Service;
        }}
    }}
}}";
    }
}