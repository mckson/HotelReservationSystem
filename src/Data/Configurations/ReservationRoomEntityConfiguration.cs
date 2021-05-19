using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class ReservationRoomEntityConfiguration : IEntityTypeConfiguration<ReservationRoomEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationRoomEntity> builder)
        {
            builder.HasKey(rr => new { rr.ReservationId, rr.RoomId });

            builder.HasOne(rr => rr.Reservation)
                .WithMany(res => res.ReservationRooms)
                .HasForeignKey(rr => rr.ReservationId);

            builder.HasOne(rr => rr.Room)
                .WithMany(room => room.ReservationRooms)
                .HasForeignKey(rr => rr.RoomId);
        }
    }
}
