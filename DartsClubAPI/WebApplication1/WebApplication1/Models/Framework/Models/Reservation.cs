﻿namespace WebApplication1.Models.Framework.Models
{
    public class Reservation
    {
       

        public Guid ID { get; set; }

        public DateTime Day { get; set; }
        
        public int Hour { get; set; }

        public User User { get; set; }

        public Guid UserId { get; set; }

        public Reservation(Guid iD, User user, DateTime day, int hour, Guid userId)
        {
            ID = iD;
            Day = day;
            Hour = hour;
            User = user;
            UserId = userId;
        }

        public Reservation()
        {
        }
    }
}