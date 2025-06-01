namespace Flighter.DTO.FlightDto
{
    public class UserBookingDto
    {
        public string Guestname { get; set; }
        public string CompanyName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public DateOnly? ReturnDepartureDate { get; set; }
        public TimeOnly? ReturnDepartureTime { get; set; }
        public DateOnly? ReturnArrivalDate { get; set; }
        public TimeOnly? ReturnArrivalTime { get; set; }
        public int DurationInMinutes { get; set; }
        public string ClassName { get; set; }
        public string FlightNumber { get; set; }
        public string Gate { get; set; }
        public List<string> SelectedSeats { get; set; }
        public string TicketCode { get; set; }
        public string BaggageAllowance { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal amount { get; set; }  
        public string PaymentStatus { get; set;}
        public int bookingid { get; set; }
        public int ticketid { get; set; }
    }

    
}
