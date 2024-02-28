using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class EmployeeArrangmentConfiguration : IEntityTypeConfiguration<EmployeeArrangment>
    {
        public void Configure(EntityTypeBuilder<EmployeeArrangment> builder)
        {
            builder.ToTable("EmployeeArrangment");

            builder.HasKey(r => r.Id);

            builder.HasOne(ea => ea.Employee)
                    .WithMany(u => u.EmployeeArrangments)
                    .HasForeignKey(ea => ea.EmployeeId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_EmployeeArrangment_Employee");

            builder.HasOne(ea => ea.Arrangement)
                .WithMany(a => a.EmployeeArrangments)
                .HasForeignKey(ea => ea.ArrangementId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_EmployeeArrangment_Arrangement");

            builder.Property(ea => ea.Rating)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}