using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.FirstName).HasMaxLength(30).IsRequired();
            
            builder.Property(a => a.LastName).HasMaxLength(30).IsRequired();

            builder.Property(a => a.BirthDate).IsRequired();

            builder.Property(a => a.ResidencePlace).HasMaxLength(30).IsRequired();

            builder
             .HasOne(e => e.User)
             .WithOne(u => u.Client)
             .HasForeignKey<User>(u => u.ClientId);

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}