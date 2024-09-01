using Iter.Core.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class ReservationStatusSeed : IEntityTypeConfiguration<ReservationStatus>
    {
        public void Configure(EntityTypeBuilder<ReservationStatus> entity)
        {
            var data = Common.Deserialize<List<ReservationStatus>>("ReservationStatus.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
