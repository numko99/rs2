public class ServiceTemplate
{
  private string entityName;
  string serviceTemplate;
  public string Generate()
  {
    return serviceTemplate.Replace("{entityName}", entityName);
  }

  public ServiceTemplate(string entityName)
  {
    this.entityName = entityName;
    this.serviceTemplate =
$@"using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Services
{{
    public class {entityName}Service : BaseCrudService<{entityName}, {entityName}UpsertRequest, {entityName}UpsertRequest, {entityName}Response, {entityName}SearchModel, {entityName}Response>, I{entityName}Service
    {{
        private readonly I{entityName}Repository {entityName.ToLower()}Repository;
        private readonly IMapper mapper;
        public {entityName}Service(I{entityName}Repository {entityName.ToLower()}Repository, IMapper mapper) : base({entityName.ToLower()}Repository, mapper)
        {{
            this.{entityName.ToLower()}Repository = {entityName.ToLower()}Repository;
            this.mapper = mapper;
        }}
    }}
}}";
  }
}

