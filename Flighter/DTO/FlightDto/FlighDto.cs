namespace Flighter.DTO.FlightDto
{
    public class FlightDto
    {

        public string From { get; set; }
        public string To { get; set; }
        public int FlightTypeId { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public TimeSpan? ReturnTime { get; set; }
        public string FlightName { get; set; }
        public string Gate { get; set; }
        public int AvailableSeats { get; set; }
        public string Price { get; set; }
        public int ClassTypeId { get; set; }
        public List<string> SeatNames { get; set; }
        public string BaggageAllowance { get; set; }
    }
}
