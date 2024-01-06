using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core;
using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.Street)
                     .HasMaxLength(100);

            builder.Property(a => a.HouseNumber)
                .HasMaxLength(20);

            builder.Property(a => a.City)
                .HasMaxLength(50);

            builder.Property(a => a.PostalCode)
                .HasMaxLength(20);

            builder.Property(a => a.Country)
                .HasMaxLength(50);

            builder.Property(a => a.DateCreated).IsRequired();

            builder.Property(a => a.DateCreated);

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}