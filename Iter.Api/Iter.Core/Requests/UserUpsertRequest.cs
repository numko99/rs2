using Iter.Core.Enum;

namespace Iter.Core{    public class UserUpsertRequest    {
        public string? Id { get; set; }

        public string? AgencyId { get; set; }        public string Email { get; set; }        public string PhoneNumber { get; set; }

        public string? BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public AddressInsertRequest Address { get; set; }        public bool? IsActive { get; set; }        public int Role { get; set; }
    }}