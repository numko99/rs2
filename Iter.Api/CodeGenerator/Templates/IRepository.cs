public class IRepositoryTemplate
{
  private string entityName;
  string iRepositoryTemplate;
  public string Generate()
  {
    return iRepositoryTemplate.Replace("{entityName}", entityName);
  }

  public IRepositoryTemplate(string entityName)
  {
    this.entityName = entityName;
    this.iRepositoryTemplate =
$@"using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Repository.Interface
{{
    public interface I{entityName}Repository : IBaseCrudRepository<{entityName}>
    {{
    }}
}}";
  }
}