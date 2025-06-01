using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class ResetPasswordDto

    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
