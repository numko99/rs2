using Iter.Core;

namespace Iter.Model
{
    public class ArrangmentSearchModel: BaseSearchModel
    {
        public string? AgencyId { get; set; }

        public string? Name { get; set; }

        public string? DateFrom { get; set; }

        public string? DateTo { get; set; }

        public int? ArrangementStatus { get; set; }

        public decimal? Rating { get; set; }

        public Guid? CurrentUserId { get; set; }
    }
}
