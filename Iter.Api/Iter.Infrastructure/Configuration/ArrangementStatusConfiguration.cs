using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Iter.Core.EntityModelss;
using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ArrangementStatusConfiguration : IEntityTypeConfiguration<ArrangementStatus>
    {
        public void Configure(EntityTypeBuilder<ArrangementStatus> builder)
        {
            builder.ToTable("ArrangementStatus");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .ValueGeneratedNever();

            builder.Property(a => a.Name)
                     .HasMaxLength(50);
        }
    }
}