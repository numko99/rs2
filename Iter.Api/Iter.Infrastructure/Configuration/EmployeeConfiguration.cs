using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;
using System.Reflection.Emit;

namespace Iter.Infrastrucure.Configurations
{
    internal class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.BirthDate).IsRequired();

            builder.Property(a => a.ResidencePlace).HasMaxLength(30).IsRequired();

            builder.Property(a => a.FirstName).HasMaxLength(30).IsRequired();

            builder.Property(a => a.LastName).HasMaxLength(30).IsRequired();

            builder.HasOne(a => a.Agency)
                 .WithMany(a => a.Employees)
                 .HasForeignKey(a => a.AgencyId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .HasConstraintName("FK_Employee_Agency");

            builder
             .HasOne(e => e.User)
             .WithOne(u => u.Employee)
             .HasForeignKey<User>(u => u.EmployeeId);

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}