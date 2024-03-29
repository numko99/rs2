﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Iter.Core.EntityModels;

namespace Iter.Infrastrucure.Configurations
{
    internal class AccomataionConfiguration: IEntityTypeConfiguration<Accommodation>
    {
        public void Configure(EntityTypeBuilder<Accommodation> builder)
        {
            builder.ToTable("Accommodation");

            builder.HasKey(r => r.Id);

            builder.Property(a => a.HotelName)
                .HasMaxLength(100);

            builder.HasOne(a => a.HotelAddress)
                     .WithMany(a => a.Accommodations)
                     .HasForeignKey(a => a.HotelAddressId)
                     .OnDelete(DeleteBehavior.NoAction)
                     .HasConstraintName("FK_Accomadation_HotelAddress");

            builder.Property(a => a.CheckInDate)
                      .IsRequired();

            builder.Property(a => a.CheckOutDate)
                      .IsRequired();

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.ModifiedAt);

            builder.Property(a => a.IsDeleted).IsRequired();
        }
    }
}