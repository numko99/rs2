using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iter.Core.RequestParameterModels;

namespace Iter.Core.Search_Models
{
    public class VerificationTokenSearchModel : BaseSearchModel
    {
        public string UserId { get; set; }
    }
}
