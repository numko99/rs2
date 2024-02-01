using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {

            builder.ToTable("Image");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.Name)
                .HasMaxLength(100);

            builder.Property(a => a.ImageContent).IsRequired();

            builder.Property(a => a.ImageThumb);
        }
    }
}