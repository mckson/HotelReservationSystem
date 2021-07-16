using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasMany(g => g.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(g => g.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.DateOfBirth)
                .IsRequired();

            builder.Ignore(user => user.Roles);
        }
    }
}
