namespace Flighter.Models.DBModels
{
    public class ClassTypeModel
    {
        public int classTypeId { get; set; }
        public string className { get; set; }
        public ICollection<TicketModel> tickets { get; set; }


    }
}
