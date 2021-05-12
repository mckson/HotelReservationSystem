using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class HotelServiceEntityConfiguration : IEntityTypeConfiguration<HotelServiceEntity>
    {
        public void Configure(EntityTypeBuilder<HotelServiceEntity> builder)
        {
            builder.HasKey(hs => new { hs.HotelId, hs.ServiceId });

            builder.HasOne(hs => hs.Hotel)
                .WithMany(h => h.HotelServices)
                .HasForeignKey(hs => hs.HotelId);

            builder.HasOne(hs => hs.Service)
                .WithMany(s => s.HotelServices)
                .HasForeignKey(hs => hs.ServiceId);
        }
    }
}
