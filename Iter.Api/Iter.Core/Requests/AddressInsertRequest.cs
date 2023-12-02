using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Requests
{
    public class AddressInsertRequest
    {
        [Required(ErrorMessage = "Polje je obavezno.")]
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        public string Country { get; set; }
    }
}
