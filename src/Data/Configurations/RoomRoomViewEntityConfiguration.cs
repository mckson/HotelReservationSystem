using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class RoomRoomViewEntityConfiguration : IEntityTypeConfiguration<RoomRoomViewEntity>
    {
        public void Configure(EntityTypeBuilder<RoomRoomViewEntity> builder)
        {
            builder.HasKey(rr => new { rr.RoomId, rr.RoomViewId });

            builder.HasOne(rr => rr.Room)
                .WithMany(r => r.RoomViews)
                .HasForeignKey(rr => rr.RoomId);

            builder.HasOne(rr => rr.RoomView)
                .WithMany(rv => rv.RoomViews)
                .HasForeignKey(rr => rr.RoomViewId);
        }
    }
}
