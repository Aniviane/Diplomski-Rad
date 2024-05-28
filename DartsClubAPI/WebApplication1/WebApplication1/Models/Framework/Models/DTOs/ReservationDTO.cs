namespace WebApplication1.Models.Framework.Models.DTOs
{
    public class ReservationDTO
    {
        public Guid? ID { get; set; }

        public DateTime Day { get; set; }

        public int Hour { get; set; }


        public Guid UserId { get; set; }


        public ReservationDTO(Reservation reservation)
        {
            ID = reservation.ID;
            Day = reservation.Day;
            Hour = reservation.Hour;
            UserId = reservation.UserId;
        }

        public ReservationDTO(Guid? iD,  DateTime day, int hour, Guid userId)
        {
            ID = iD;
            Day = day;
            Hour = hour;
            UserId = userId;
        }

        public ReservationDTO()
        {
        }
    }
}
