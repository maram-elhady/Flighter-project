namespace Flighter.Models.DBModels
{
    public class FlightSeatModel
    {
        public int SeatId { get; set; }
        public string? UserId { get; set; }
        public string SeatName { get; set; }
        public bool isBooked { get; set; }
        public int ticketId { get; set; }
        public TicketModel ticket { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<BookingSeatModel> BookingSeats { get; set; }

    }
}
