namespace Iter.Core.Search_Models
{
    public class EmployeeArrangementSearchModel : BaseSearchModel
    {
        public Guid? ArrangementId { get; set; }

        public Guid? EmployeeId { get; set; }

        public bool? ReturnActiveArrangements { get; set; }
    }
}
