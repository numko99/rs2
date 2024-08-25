using Iter.Core;

namespace Iter.Model
{
    public class EmployeeArrangementSearchModel : BaseSearchModel
    {
        public string? Name { get; set; }

        public Guid? ArrangementId { get; set; }

        public Guid? EmployeeId { get; set; }

        public bool? ReturnAll { get; set; }

        public bool? ReturnActiveArrangements { get; set; }
    }
}
