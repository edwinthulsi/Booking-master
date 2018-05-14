using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Booking_master.Models
{
        public class BookingDbContext : DbContext
        {
            public BookingDbContext() : base("ApexBookings")
        {
            Database.SetInitializer<BookingDbContext>(new CreateDatabaseIfNotExists<BookingDbContext>());
        }

            public DbSet<Registration> registrations { get; set; }
            public DbSet<Booking> bookings { get; set; }
        }
    }
