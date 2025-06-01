using System.ComponentModel.DataAnnotations;

namespace CompanyDashboard.Models
{
    public class AddFlightDto
    {
        public string? CompanyName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int FlightTypeId { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public DateOnly? returnDepartureDate { get; set; }
        public TimeOnly? returnDepartureTime { get; set; }
        public DateOnly? returnArrivalDate { get; set; }
        public TimeOnly? returnArrivalTime { get; set; }
        public string FlightName { get; set; }
        public string Gate { get; set; }
        public int AvailableSeats { get; set; }
        public string Price { get; set; }
        public int ClassTypeId { get; set; }
        public string offer_percentage { get; set; } 
        public List<string> SeatNames { get; set; }
        public string BaggageAllowance { get; set; }
    }
}
