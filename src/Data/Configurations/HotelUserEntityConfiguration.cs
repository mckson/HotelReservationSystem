using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class HotelUserEntityConfiguration : IEntityTypeConfiguration<HotelUserEntity>
    {
        public void Configure(EntityTypeBuilder<HotelUserEntity> builder)
        {
            builder.HasKey(hu => new { hu.HotelId, hu.UserId });

            builder.HasOne(hu => hu.Hotel)
                .WithMany(hotel => hotel.HotelUsers)
                .HasForeignKey(hu => hu.HotelId);

            builder.HasOne(hu => hu.User)
                .WithMany(user => user.HotelUsers)
                .HasForeignKey(hu => hu.UserId);
        }
    }
}
