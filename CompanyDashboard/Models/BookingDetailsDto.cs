namespace CompanyDashboard.Models
{
    public class BookingDetailsDto

    {
        public int ticketid { get; set; }
        public string FlightNumber { get; set; }

        public List<string> ReservedSeats { get; set; }

        public DateTime BookingDate { get; set; }

        public string PaymentStatus { get; set; }

        public DateOnly DepartureDate { get; set; }
        public string userEmail {  get; set; }
        public string CompanyName { get; set; }

    }


}
