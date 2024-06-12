using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;
using Iter.Core.EntityModelss;

namespace Iter.Infrastrucure.Configurations
{
    internal class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
    {
        public void Configure(EntityTypeBuilder<VerificationToken> builder)
        {
            builder.ToTable("VerificationToken");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token)
                    .HasMaxLength(5);

            builder.Property(r => r.ExpiryDate);

            builder.Property(r => r.VerificationTokenType)
              .HasMaxLength(30);

            builder.HasOne(r => r.User)
                .WithMany(u => u.VerificationTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_VerificationToken_User");
        }
    }
}