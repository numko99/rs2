public class IServiceTemplate
{
  private string entityName;
  string iServiceTemplate;
  public string Generate()
  {
    return iServiceTemplate.Replace("{entityName}", entityName);
  }

  public IServiceTemplate(string entityName)
  {
    this.entityName = entityName;
    this.iServiceTemplate =
$@"using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using Iter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Services.Interface
{{
    public interface I{entityName}Service : IBaseCrudService<{entityName}, {entityName}UpsertRequest, {entityName}UpsertRequest, {entityName}Response>
    {{
    }}
}}";
  }
}