using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Dto
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string? FirstName { get; init; }

        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; init; }

        [Required(ErrorMessage = "UserName is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "BirthDate is required")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "BirthPlace is required")]
        public string BirthPlace { get; set; }
    }
}
