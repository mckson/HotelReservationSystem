using System;
using HotelReservation.Data.Configurations;
using HotelReservation.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data
{
    public class HotelContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
    {
        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }

        public DbSet<HotelEntity> Hotels { get; set; }

        public DbSet<RoomEntity> Rooms { get; set; }

        public DbSet<LocationEntity> Locations { get; set; }

        public DbSet<ReservationEntity> Reservations { get; set; }

        public DbSet<ServiceEntity> Services { get; set; }

        public DbSet<ReservationRoomEntity> ReservationRooms { get; set; }

        public DbSet<ReservationServiceEntity> ReservationServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new HotelEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoomEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LocationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationRoomEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationServiceEntityConfiguration());
        }
    }
}