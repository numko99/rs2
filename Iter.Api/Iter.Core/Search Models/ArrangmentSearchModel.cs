namespace Iter.Core.Search_Models
{
    public class ArrangmentSearchModel: BaseSearchModel
    {
        public string? AgencyId { get; set; }

        public string? Name { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
