using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data
{
    public class HotelContext : DbContext
    {
        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        //public DbSet<Guest> Guests { get; set; }
        //public DbSet<Person> Persons { get; set; }

        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(b => b.MigrationsAssembly("HotelAPI"));
        }
    }
}
