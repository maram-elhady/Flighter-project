using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flighter.Models.DBModels
{
    public class TicketModel
    {
        [Key]
        public int TicketId { get; set; }

        [ForeignKey("flightId")]
        public int flightId { get; set; }
        public  int classTypeId { get; set; }
        public int  scheduleId { get; set; }
        public string price { get; set; }
        public string gate { get; set; }
        public string flightNumber { get; set; }
        public string offer_percentage { get; set; } 
        public int availableSeats { get; set; }
        public int totalSeats { get; set; } 
        public string BaggageAllowance { get; set; }
        public FlightModel flight { get; set; }
        public ClassTypeModel classType { get; set; }
        public ScheduleModel schedule { get; set; }
        public int CompanyId { get; set; }
        public string userId { get; set; }
        public CompanyModel Company { get; set; }
        public ApplicationUser Admin { get; set; }
        public ICollection<BookingModel> bookings { get; set; }
        public ICollection<FlightSeatModel> seats { get; set; }

    }
}
