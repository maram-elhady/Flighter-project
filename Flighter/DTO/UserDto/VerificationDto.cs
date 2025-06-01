using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class VerificationDto
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
