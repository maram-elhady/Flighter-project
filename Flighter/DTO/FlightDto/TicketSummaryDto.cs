namespace Flighter.DTO.FlightDto
{
    public class TicketSummaryDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public string FlightNumber { get; set; }
        public string Gate { get; set; }
        public string ClassName { get; set;}
        public List<string> SelectedSeats { get; set; }
        public string TicketCode { get; set; }
    }
}
