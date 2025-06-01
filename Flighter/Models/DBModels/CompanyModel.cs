namespace Flighter.Models.DBModels
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<TicketModel> Tickets { get; set; }
        public ICollection<FlightModel> Flights { get; set; }
    }
}