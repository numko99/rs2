using Iter.Core.Enum;

namespace Iter.Core.EntityModels{    public class UserResponse    {
        public Guid Id { get; set; }

        public Guid? ClientId { get; set; }

        public Guid? EmployeeId { get; set; }

        public AgencyResponse Agency { get; set; }        public string Email { get; set; }        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string ResidencePlace { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsActive { get; set; }        public Roles Role { get; set; }    }}