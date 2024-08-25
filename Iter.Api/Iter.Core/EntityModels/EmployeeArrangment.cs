namespace Iter.Core.EntityModels
{
    public class EmployeeArrangment : BaseEntity
    {
        public Employee Employee { get; set; }

        public Guid EmployeeId { get; set; }

        public Arrangement Arrangement { get; set; }

        public Guid ArrangementId { get; set; }

        public decimal Rating { get; set; }
    }
}
