using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class GuestEntityConfiguration : IEntityTypeConfiguration<GuestEntity>
    {
        public void Configure(EntityTypeBuilder<GuestEntity> builder)
        {
            builder.HasOne(g => g.Reservation)
                .WithOne(r => r.Guest)
                .HasForeignKey<ReservationEntity>(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(g => g.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.DateOfBirth)
                .IsRequired();
        }
    }
}
