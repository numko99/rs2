using Iter.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Iter.Core.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public Guid? AgencyId { get; set; }

        public Agency Agency { get; set; }

        public bool IsActive { get; set; }

        public List<Reservation> Reservations { get; set; }

        public List<EmployeeArrangment> EmployeeArrangments { get; set; }
    }
}
