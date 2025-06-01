namespace Flighter.Models.DBModels
{
    public class FlightTypeModel
    {
        public int FlightTypeId { get; set; }
        public string FlightName { get; set; }
        public ICollection<FlightModel> Flights{ get; set; }
    }
}
