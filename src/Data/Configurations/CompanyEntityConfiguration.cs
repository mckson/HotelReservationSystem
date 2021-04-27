using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservation.Data.Configurations
{
    public class CompanyEntityConfiguration : IEntityTypeConfiguration<CompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            builder.Property(c => c.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(c => c.Hotels)
                .WithOne(h => h.Company)
                .HasForeignKey(h => h.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
