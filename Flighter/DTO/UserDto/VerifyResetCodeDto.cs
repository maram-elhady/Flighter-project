using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class VerifyResetCodeDto
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }
    }
}