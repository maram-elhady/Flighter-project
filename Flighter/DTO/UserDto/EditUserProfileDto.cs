using System.ComponentModel.DataAnnotations;

namespace Flighter.DTO.UserDto
{
    public class EditUserProfileDto
    {
        public IFormFile? ProfilePhoto { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Country { get; set; }
    }
}
