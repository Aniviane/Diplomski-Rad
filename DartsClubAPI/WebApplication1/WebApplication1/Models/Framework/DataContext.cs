﻿using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Framework.Models;

namespace WebApplication1.Models.Framework_Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        }
    }
}