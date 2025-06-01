using Flighter.DTO.UserDto;
using Flighter.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flighter.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string ConfirmPassword { get; set; }
        public List<RefreshTokens>? RefreshTokens { get; set; } 
        public string? ProfilePhoto { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; } = "Egypt";

        [ForeignKey("CompanyId")]
        public int? CompanyId { get; set; }
        public CompanyModel Company { get; set; }
        public ICollection<TicketModel> tickets { get; set; }
        //public ICollection<PaymentModel> payments { get; set; }
       
        public ICollection<BookingModel> bookings { get; set; }
        public ICollection<FlightSeatModel> seats { get; set; }

    }
}
