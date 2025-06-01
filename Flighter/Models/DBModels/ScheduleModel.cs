namespace Flighter.Models.DBModels
{
    public class ScheduleModel
    {
        public int ScheduleId { get; set; }
        public int flightId { get; set; }
        public DateOnly departureDate { get; set; }
        public TimeOnly departureTime { get; set; }
        public DateOnly arrivalDate { get; set; }
        public TimeOnly arrivalTime { get; set; }
        public DateOnly? returnDepartureDate { get; set; }
        public TimeOnly? returnDepartureTime { get; set; }
        public DateOnly? returnArrivalDate { get; set; }
        public TimeOnly? returnArrivalTime { get; set; }
        public FlightModel flight {  get; set; }
        public ICollection<TicketModel> tickets { get; set; }
    }
}
