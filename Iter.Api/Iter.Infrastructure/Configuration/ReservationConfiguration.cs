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


            builder.HasOne(r => r.DepartureCity)
              .WithMany(u => u.Reservations)
              .HasForeignKey(r => r.DepartureCityId)
              .OnDelete(DeleteBehavior.NoAction)
              .HasConstraintName("FK_Reservation_DepartureCity");

            builder.Property(r => r.TransactionId)
               .HasMaxLength(40);

            builder.HasOne(r => r.Client)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Reservation_Client");

            builder.HasOne(r => r.Arrangement)
                .WithMany(a => a.Reservations)
                .HasForeignKey(r => r.ArrangmentId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Reservation_Arrangement");

            builder.HasOne(r => r.ReservationStatus)
                .WithMany()
                .HasForeignKey(r => r.ReservationStatusId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Reservation_ReservationStatus");

            builder.HasOne(r => r.ArrangementPrice)
            .WithMany(a => a.Reservations)
            .HasForeignKey(r => r.ArrangementPriceId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Reservation_ArrangementPrice");

            builder.Property(r => r.TotalPaid)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt).IsRequired();

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}