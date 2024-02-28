using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ArrangementImageConfiguration : IEntityTypeConfiguration<ArrangementImage>
    {
        public void Configure(EntityTypeBuilder<ArrangementImage> builder)
        {

            builder.ToTable("ArrangementImage");

            builder.HasKey(r => r.Id);

            builder.HasOne(d => d.Image)
                .WithMany(a => a.ArrangementImages)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ArrangementImage_Image");

            builder.HasOne(d => d.Arrangement)
                .WithMany(a => a.ArrangementImages)
                .HasForeignKey(d => d.ArrangementId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ArrangementImage_Arrangement");

            builder.Property(a => a.IsMainImage).IsRequired();

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}