namespace Iter.Core.EntityModels
{
    public class Employee : Person
    {
        public Guid AgencyId { get; set; }

        public Agency Agency { get; set; }

        public List<EmployeeArrangment> EmployeeArrangments { get; set; }
    }
}
