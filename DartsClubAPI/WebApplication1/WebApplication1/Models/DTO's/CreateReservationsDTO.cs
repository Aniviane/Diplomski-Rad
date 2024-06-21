namespace WebApplication1.Models.DTO_s
{
    public class CreateReservationsDTO
    {
        public CreateReservationsDTO( DateTime day, Guid userID)
        {
            Day = day;
            Hours = new List<int>();
            UserId = userID;
        }



        public Guid UserId { get; set; }

        public DateTime Day { get; set; }


        public List<int> Hours { get; set;}
    }
}
