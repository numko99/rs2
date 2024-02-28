using Iter.Core.EntityModels;

namespace Iter.Core
{
    public class AgencyResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AddressResponse Address { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Website { get; set; }

        public string LicenseNumber { get; set; }

        public bool IsActive { get; set; }

        public ImageResponse Logo { get; set; }

        public decimal Rating { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
