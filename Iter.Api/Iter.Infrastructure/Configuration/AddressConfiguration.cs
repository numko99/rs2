using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(r => r.Id);

            builder.HasOne(a => a.City)
             .WithMany(a => a.Address)
             .HasForeignKey(a => a.CityId)
             .OnDelete(DeleteBehavior.NoAction)
             .HasConstraintName("FK_Address_City");

            builder.Property(a => a.Street)
                     .HasMaxLength(100);

            builder.Property(a => a.HouseNumber)
                .HasMaxLength(20);

            builder.Property(a => a.PostalCode)
                .HasMaxLength(20);

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}