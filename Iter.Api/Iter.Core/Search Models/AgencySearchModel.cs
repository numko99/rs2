namespace Iter.Core.Search_Models
{
    public class UserSearchModel: BaseSearchModel
    {
        public string? Name { get; set; }

        public string? AgencyId { get; set; }
        
        public int? RoleId { get; set; }

    }
}
