using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace HotelReservation.Data.Configurations
{
    public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder.HasMany(r => r.Guests)
                .WithOne(g => g.Room)
                .HasForeignKey(g => g.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Reservation)
                .WithOne(res => res.Room)
                .HasForeignKey<ReservationEntity>(res => res.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
