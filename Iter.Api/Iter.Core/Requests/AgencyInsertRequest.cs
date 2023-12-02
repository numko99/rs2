using Iter.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Requests
{
    public class AgencyInsertRequest
    {
        [Required(ErrorMessage = "Polje je obavezno.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        public string ContactPhone { get; set; }

        public string Website { get; set; }

        [Required(ErrorMessage = "Polje je obavezno.")]
        public string LicenseNumber { get; set; }

        public string LogoUrl { get; set; }




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
