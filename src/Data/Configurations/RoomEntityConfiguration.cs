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
            builder.HasOne(r => r.Guest)
                .WithOne(g => g.Room)
                .HasForeignKey<GuestEntity>(g => g.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Reservation)
                .WithMany(res => res.Rooms)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
