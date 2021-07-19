using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasAlternateKey(user => user.Email);

            builder.HasMany(user => user.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.Email)
                .HasPrincipalKey(u => u.Email)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(user => user.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(user => user.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(user => user.DateOfBirth)
                .IsRequired();

            builder.Ignore(user => user.Roles);
        }
    }
}
