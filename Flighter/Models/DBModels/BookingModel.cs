using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Flighter.Models.DBModels
{
    public class BookingModel
    {
        [Key]
        public int BookingId { get; set; }
        public int ticketId { get; set; }
        public string userId { get; set; }
        public DateTime bookingDate { get; set; }
        public string paymentStatus { get; set; }
        public TicketModel ticket { get; set; }
        public ApplicationUser user { get; set; }
        public PaymentModel payment { get; set; }
        public ICollection<BookingSeatModel> BookingSeats { get; set; }

    }
}
