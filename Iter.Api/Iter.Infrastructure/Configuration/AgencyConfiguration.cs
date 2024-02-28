using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class AgencyConfiguration : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.ToTable("Agency");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.Name)
                   .HasMaxLength(100);

            builder.Property(a => a.ContactEmail)
                .HasMaxLength(100);

            builder.Property(a => a.ContactPhone)
                .HasMaxLength(20);

            builder.Property(a => a.Website)
                .HasMaxLength(100);

            builder.Property(a => a.LicenseNumber)
                .HasMaxLength(50);

            builder.HasOne(a => a.Address)
                .WithMany(a => a.Agencies)
                .HasForeignKey(a => a.AddressId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Agency_Address");

            builder.HasOne(a => a.Image)
                .WithMany(a => a.Agencies)
                .HasForeignKey(a => a.ImageId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Agency_Image");

            builder.Property(a => a.Rating)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}