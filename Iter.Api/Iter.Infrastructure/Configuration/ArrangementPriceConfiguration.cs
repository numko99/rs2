using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ArrangementPriceConfiguration : IEntityTypeConfiguration<ArrangementPrice>
    {
        public void Configure(EntityTypeBuilder<ArrangementPrice> builder)
        {

            builder.ToTable("ArrangementPrice");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.AccommodationType)
            .HasMaxLength(100);

            builder.HasOne(d => d.Arrangement)
                .WithMany(a => a.ArrangementPrices)
                .HasForeignKey(d => d.ArrangementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArrangementPrice_Arrangement");


            builder.Property(a => a.DateCreated).IsRequired();

            builder.Property(a => a.DateCreated);

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}