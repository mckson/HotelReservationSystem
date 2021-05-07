using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class LocationEntityConfiguration : IEntityTypeConfiguration<LocationEntity>
    {
        public void Configure(EntityTypeBuilder<LocationEntity> builder)
        {
            builder.Property(l => l.Country)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(l => l.Region)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(l => l.City)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(l => l.Street)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(l => l.BuildingNumber)
                .IsRequired();
        }
    }
}
