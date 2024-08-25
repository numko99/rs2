using Iter.Core.EntityModelss;
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

        public DateTime? CreatedAt{ get; set; }

        public DateTime? ModifiedAt { get; set; }

        public List<VerificationToken> VerificationTokens { get; set; }

    }
}
