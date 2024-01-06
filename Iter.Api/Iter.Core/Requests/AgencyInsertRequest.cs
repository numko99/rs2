using Iter.Core.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Requests
{
    public class AgencyInsertRequest
    {
        public string Name { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Website { get; set; }

        public string LicenseNumber { get; set; }

        public byte[]? Logo { get; set; }

        public byte[]? LogoThumb { get; set; }


        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
