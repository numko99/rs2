using Iter.Core.EntityModels;
using Iter.Core.EntityModelss;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class CountrySeed : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            var data = Common.Deserialize<List<Country>>("Countries.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
