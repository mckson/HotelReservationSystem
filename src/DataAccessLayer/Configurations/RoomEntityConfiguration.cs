using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class RoomEntityConfiguration : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder.HasOne(x => x.Hotel)
                .WithMany(x => x.Rooms)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
