using Iter.Core.EntityModels;

namespace Iter.Core.EntityModels
{
    public class Agency : BaseEntity
    {
        public string Name { get; set; }

        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Website { get; set; }

        public string LicenseNumber { get; set; }

        public bool IsActive { get; set; }

        public Guid? ImageId { get; set; }

        public Image? Image { get; set; }

        public decimal Rating { get; set; }


        public List<Employee> Employees { get; set; }

        public List<Arrangement> Arrangements { get; set; }
    }
}
