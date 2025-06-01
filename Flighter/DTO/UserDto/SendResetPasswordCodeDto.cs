using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class SendResetPasswordCodeDto
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
    }
}