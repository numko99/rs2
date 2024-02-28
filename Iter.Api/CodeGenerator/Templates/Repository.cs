public class RepositoryTemplate
{
  private string entityName;
  string repositoryTemplate;
  public string Generate()
  {
    return repositoryTemplate.Replace("{entityName}", entityName);
  }

  public RepositoryTemplate(string entityName)
  {
    this.entityName = entityName;
    this.repositoryTemplate =
$@"using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Repository
{{
    public class {entityName}Repository : BaseCrudRepository<{entityName}, {entityName}UpsertRequest, {entityName}UpsertRequest, {entityName}Response>, I{entityName}Repository
    {{
        private readonly IterContext dbContext;
        public {entityName}Repository(IterContext dbContext) : base(dbContext)
        {{
            this.dbContext = dbContext;
        }}
    }}
}}";
  }
}