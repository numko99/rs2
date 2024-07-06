namespace Iter.Core.RequestParameterModels
{
    public class EmployeeArrangementRequestParameters : BaseSearchModel
    {
        public string? Name { get; set; }

        public Guid? ArrangementId { get; set; }

        public Guid? EmployeeId { get; set; }

        public bool? ReturnAll { get; set; }

        public bool? ReturnActiveArrangements { get; set; }
    }
}
