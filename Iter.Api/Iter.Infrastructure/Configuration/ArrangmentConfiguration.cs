using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.Models;

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
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Arrangement_Agency");

            builder.Property(a => a.Name)
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(500);

            builder.Property(a => a.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.Capacity)
                .IsRequired();

            builder.Property(a => a.StartDate)
                .IsRequired();

            builder.Property(a => a.EndDate)
                .IsRequired();

            builder.Property(a => a.DateCreated).IsRequired();

            builder.Property(a => a.DateCreated);

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}