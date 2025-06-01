using System.ComponentModel.DataAnnotations;

namespace Flighter.Models.DBModels
{
    public class FlightModel
    {
        [Key]
        public int FlightId { get; set; }
        public string fromLocation { get; set; }
        public string toLocation { get; set; }
        public int FlightTypeId { get; set; }
        public int CompanyId { get; set; }
        public CompanyModel Company { get; set; }
        public  FlightTypeModel FlightType { get; set; }
        public ICollection<ScheduleModel> schedules { get; set; }
        public ICollection<TicketModel> tickets { get; set; }
    }
}
