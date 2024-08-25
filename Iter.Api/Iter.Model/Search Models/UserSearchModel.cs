using Iter.Core;

namespace Iter.Model
{
    public class UserSearchModel: BaseSearchModel
    {
        public string? Name { get; set; }

        public string? AgencyId { get; set; }
        
        public int? RoleId { get; set; }

    }
}
