using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class ReservationServiceEntityConfiguration : IEntityTypeConfiguration<ReservationServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationServiceEntity> builder)
        {
            builder.HasKey(rs => new { rs.ReservationId, rs.ServiceId });

            builder.HasOne(rs => rs.Reservation)
                .WithMany(r => r.ReservationServices)
                .HasForeignKey(rs => rs.ReservationId);

            builder.HasOne(rs => rs.Service)
                .WithMany(s => s.ReservationServices)
                .HasForeignKey(rs => rs.ServiceId);
        }
    }
}
