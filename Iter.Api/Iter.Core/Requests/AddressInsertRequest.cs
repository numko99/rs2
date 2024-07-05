using System.ComponentModel.DataAnnotations;

namespace Iter.Core
{
    public class AddressInsertRequest
    {
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string CityId { get; set; }

        public string PostalCode { get; set; }
    }
}
