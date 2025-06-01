using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Flighter.Models.DBModels
{
    public class BookingSeatModel
    {
        [Key]
        public int BookingSeatId { get; set; }
        public int BookingId { get; set; }
        public int SeatId { get; set; }

        // Navigation properties
        [ForeignKey("BookingId")]
        public BookingModel Booking { get; set; }

        [ForeignKey("SeatId")]
        public FlightSeatModel Seat { get; set; }
    }
}
