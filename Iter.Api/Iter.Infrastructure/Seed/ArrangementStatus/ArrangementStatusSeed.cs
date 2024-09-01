using Iter.Core.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class ArrangementStatusSeed : IEntityTypeConfiguration<ArrangementStatus>
    {
        public void Configure(EntityTypeBuilder<ArrangementStatus> entity)
        {
            var data = Common.Deserialize<List<ArrangementStatus>>("ArrangemetStatus.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
