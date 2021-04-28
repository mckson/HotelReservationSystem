using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class HotelEntityConfiguration : IEntityTypeConfiguration<HotelEntity>
    {
        public void Configure(EntityTypeBuilder<HotelEntity> builder)
        {
            builder.HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Hotel)
                .HasForeignKey(x => x.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Location)
                .WithOne(l => l.Hotel)
                .HasForeignKey<LocationEntity>(l => l.HotelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(h => h.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
