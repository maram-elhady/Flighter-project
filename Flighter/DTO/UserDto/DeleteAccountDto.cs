using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class DeleteAccountDto
    {
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
