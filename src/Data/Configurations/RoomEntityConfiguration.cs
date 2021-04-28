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
            builder.HasOne(r => r.User)
                .WithOne(g => g.Room)
                .HasForeignKey<UserEntity>(g => g.RoomId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Reservation)
                .WithMany(res => res.Rooms)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.RoomNumber)
                .IsRequired();

            builder.Property(r => r.FloorNumber)
                .IsRequired();

            builder.Property(r => r.Capacity)
                .IsRequired();

            builder.Property(r => r.IsEmpty)
                .HasDefaultValue(true)
                .IsRequired();
        }
    }
}
