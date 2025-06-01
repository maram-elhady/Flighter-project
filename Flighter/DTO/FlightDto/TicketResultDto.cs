namespace Flighter.DTO.FlightDto
{
    public class TicketResultDto
    {
        public int TicketId { get; set; }
        public string Price { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public int DurationInMinutes { get; set; }

        public string CompanyName { get; set; }
    }
}
