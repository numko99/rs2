using Iter.Core.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class AgencySeed : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> entity)
        {
            var data = Common.Deserialize<List<Agency>>("Agencies.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
