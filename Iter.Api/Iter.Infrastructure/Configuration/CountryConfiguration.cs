using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Iter.Core.Models;

namespace Iter.Infrastrucure.Configurations
{
    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Country");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.Name)
                     .HasMaxLength(50);
        }
    }
}