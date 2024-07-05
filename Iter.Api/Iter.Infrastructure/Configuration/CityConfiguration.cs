using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.Name)
                     .HasMaxLength(50);

            builder.HasOne(d => d.Country)
              .WithMany(a => a.Cities)
              .HasForeignKey(d => d.CountryId)
              .OnDelete(DeleteBehavior.NoAction)
              .HasConstraintName("FK_City_Coutnry");
        }
    }
}