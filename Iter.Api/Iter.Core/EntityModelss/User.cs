using Microsoft.AspNetCore.Identity;

namespace Iter.Core.EntityModels
{
    public class User : IdentityUser
    {
        public Guid? EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public Guid? ClientId { get; set; }

        public Client? Client { get; set; }

        public bool IsActive { get; set; }

        public int Role { get; set; }

    }
}
