﻿namespace WebApplication1.Models.Framework.Models
{
    public class Picture
    {
        public Picture()
        {
        }

        public Picture(Guid id, string imagePath, Guid UserId)
        {
            Id = id;
            this.imagePath = imagePath;
        }

        public Guid? Id { get; set; }

        public string imagePath { get; set; }

        public Guid? UserId { get; set; }

    }
}