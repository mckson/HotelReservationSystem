using HotelReservation.Data.Configurations;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data
{
    public class HotelContext : IdentityDbContext<UserEntity>
    {
        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<HotelEntity> Hotels { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<UserEntity> SystemUsers { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new HotelEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoomEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LocationEntityConfiguration());
        }
    }
}