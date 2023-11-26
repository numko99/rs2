using System.ComponentModel.DataAnnotations;

namespace Iter.Core.Dto
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Polje je obavezno")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Polje je obavezno")]
        public string? Password { get; init; }
    }
}
