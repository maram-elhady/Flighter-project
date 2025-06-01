namespace CompanyDashboard.Models
{
    public class AdminTicketDto
    {
        public int TicketId { get; set; }
        public string FlightNumber { get; set; }
        public string Gate { get; set; }
        public string Price { get; set; }
        public string OfferPercentage { get; set; } = "0" ;
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
        public string BaggageAllowance { get; set; }
        public string ClassType { get; set; }
        public CompanyInfo Company {  get; set; }
        public FlightInfo Flight { get; set; }
        public ScheduleInfo Schedule { get; set; }
        public List<SeatInfo> SeatNames { get; set; }

        public class CompanyInfo
        {
            public string CompanyName { get; set; }
        }

        public class FlightInfo
        {
            public int FlightId { get; set; }
            public string FromLocation { get; set; }
            public string ToLocation { get; set; }
            public string FlightType { get; set; }
        }

        public class ScheduleInfo
        {
            public DateOnly DepartureDate { get; set; }
            public TimeOnly DepartureTime { get; set; }
            public DateOnly ArrivalDate { get; set; }
            public TimeOnly ArrivalTime { get; set; }
            public DateOnly? ReturnDepartureDate { get; set; }
            public TimeOnly? ReturnDepartureTime { get; set; }
            public DateOnly? ReturnArrivalDate { get; set; }
            public TimeOnly? ReturnArrivalTime { get; set; }
            public int DurationInMinutes    { get; set; }
        }

        public class SeatInfo
        {
            public string SeatName { get; set; }
            public bool IsBooked { get; set; }
        }
    }
}
