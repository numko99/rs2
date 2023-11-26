namespace Iter.Core.Models
{
    public class Agency
    {
        public Agency()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Website { get; set; }

        public string LicenseNumber { get; set; }

        public bool IsActive { get; set; }

        public string LogoUrl { get; set; }

        public decimal Rating { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }


        public List<User> Users{ get; set; }

        public List<Arrangement> Arrangements{ get; set; }
    }
}
