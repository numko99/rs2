using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ArrangementConfiguration : IEntityTypeConfiguration<Arrangement>
    {
        public void Configure(EntityTypeBuilder<Arrangement> builder)
        {
            builder.ToTable("Arrangement");

            builder.HasKey(r => r.Id);

            builder.HasOne(a => a.Agency)
                   .WithMany(a => a.Arrangements)
                   .HasForeignKey(a => a.AgencyId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("FK_Arrangement_Agency");

            builder.Property(a => a.Name)
                .HasMaxLength(100);

            builder.Property(a => a.Description);

            builder.Property(a => a.Rating);

            builder.Property(a => a.ShortDescription)
            .HasMaxLength(500);

            builder.Property(a => a.StartDate)
                .IsRequired();

            builder.Property(a => a.EndDate);

            builder.HasOne(a => a.ArrangementStatus)
                           .WithMany(a => a.Arrangements)
                           .HasForeignKey(a => a.ArrangementStatusId)
                           .OnDelete(DeleteBehavior.NoAction)
                           .HasConstraintName("FK_Arrangement_ArrangementStatus");

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}