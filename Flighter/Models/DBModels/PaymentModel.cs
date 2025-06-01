using System.ComponentModel.DataAnnotations.Schema;

namespace Flighter.Models.DBModels
{
    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public string Payment_Intent_Id { get; set; }
        public string Amount { get; set;}
       
        public BookingModel booking { get; set; }
    }
}
