namespace Flighter.DTO.FlightDto
{
    public class GetTicketDto
    {
        public int FlightTypeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int NoOfTravelers { get; set; }
        public int ClassTypeId { get; set; }
        public List<int>? AirlineIds { get; set; } 
        public bool FilterCheapest { get; set; }
        public bool FilterFastest { get; set; }

    }
}
