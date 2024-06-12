using Iter.Core.Enum;

namespace Iter.Core{    public class CurrentUserUpsertRequest    {
        public string Email { get; set; }        public string? PhoneNumber { get; set; }

        public string? BirthDate { get; set; }

        public string? ResidencePlace { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }}