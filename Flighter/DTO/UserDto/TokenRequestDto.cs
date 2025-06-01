using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class TokenRequestDto
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
