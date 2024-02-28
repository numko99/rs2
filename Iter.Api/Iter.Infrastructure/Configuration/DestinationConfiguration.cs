using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {

            builder.ToTable("Destination");

            builder.HasKey(r => r.Id);

            builder.Property(d => d.City)
                     .HasMaxLength(50);

            builder.Property(d => d.Country)
                .HasMaxLength(50);

            builder.HasOne(d => d.Accommodation)
                .WithMany(a => a.Destinations)
                .HasForeignKey(d => d.AccommodationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Destination_Accommodation");

            builder.HasOne(d => d.Arrangement)
                .WithMany(a => a.Destinations)
                .HasForeignKey(d => d.ArrangementId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Destination_Arrangement");

            builder.Property(d => d.ArrivalDate)
                .IsRequired();

            builder.Property(d => d.DepartureDate)
                .IsRequired();

            builder.Property(d => d.IsOneDayTrip)
                .IsRequired();

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}