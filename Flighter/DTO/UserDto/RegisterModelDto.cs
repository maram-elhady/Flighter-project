using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Flighter.DTO.UserDto
{
    public class RegisterModelDto
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [MaxLength(128)]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [MaxLength(128)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match. Please try again")]
        public string ConfirmPassword { get; set; }

        /*public bool IsEmailValid()
        {
            return Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.(com)$");
        }*/

        [JsonIgnore]
        public string? VerificationCode { get; set; }

    }

}
