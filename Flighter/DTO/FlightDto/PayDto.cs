using System.Diagnostics.CodeAnalysis;

namespace Flighter.DTO.FlightDto
{
    public class PayDto
    {
        public string UserId { get; set; }
        public int TicketId { get; set; }
        public List<int> SeatsId { get; set; }
        public bool PayNow { get; set; } = false;
        //public string PaymentIntentId { get; set; }
        //public string Amount { get; set; }
    }
}
