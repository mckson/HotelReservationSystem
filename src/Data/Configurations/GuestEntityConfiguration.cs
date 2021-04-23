using System;
using System.Collections.Generic;
using System.Text;
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
        }
    }
}
