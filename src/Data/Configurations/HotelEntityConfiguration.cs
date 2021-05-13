using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class HotelEntityConfiguration : IEntityTypeConfiguration<HotelEntity>
    {
        public void Configure(EntityTypeBuilder<HotelEntity> builder)
        {
            builder.HasOne(h => h.Location)
                .WithOne(l => l.Hotel)
                .HasForeignKey<LocationEntity>(l => l.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Services)
                .WithOne(s => s.Hotel)
                .HasForeignKey(s => s.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Managers)
                .WithOne(x => x.Hotel)
                .HasForeignKey(x => x.HotelId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Reservations)
                .WithOne(res => res.Hotel)
                .HasForeignKey(res => res.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(h => h.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
