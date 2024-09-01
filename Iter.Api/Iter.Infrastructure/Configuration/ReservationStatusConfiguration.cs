using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Iter.Core.EntityModelss;
using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ReservationStatusConfiguration : IEntityTypeConfiguration<ReservationStatus>
    {
        public void Configure(EntityTypeBuilder<ReservationStatus> builder)
        {
            builder.ToTable("ReservationStatus");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .ValueGeneratedNever();

            builder.Property(a => a.Name)
                     .HasMaxLength(50);
        }
    }
}