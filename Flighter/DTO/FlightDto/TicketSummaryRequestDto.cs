namespace Flighter.DTO.FlightDto
{
    public class TicketSummaryRequestDto
    {
        public int TicketId { get; set; }
        public List<string> SelectedSeats { get; set; }
    }
}
