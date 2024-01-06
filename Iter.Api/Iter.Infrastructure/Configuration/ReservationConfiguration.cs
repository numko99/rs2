using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservation");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReservationNumber)
                    .HasMaxLength(50);

            builder.Property(r => r.DeparturePlace)
                .HasMaxLength(100);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservation_User");

            builder.HasOne(r => r.Arrangement)
                .WithMany(a => a.Reservations)
                .HasForeignKey(r => r.ArrangmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservation_Arrangement");

            builder.HasOne(r => r.Status)
                .WithMany()
                .HasForeignKey(r => r.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservation_ReservationStatus");

            builder.Property(r => r.TotalPaid)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.DateCreated).IsRequired();

            builder.Property(a => a.DateCreated);

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}