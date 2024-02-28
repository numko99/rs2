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
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ArrangementPrice_Arrangement");


            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}