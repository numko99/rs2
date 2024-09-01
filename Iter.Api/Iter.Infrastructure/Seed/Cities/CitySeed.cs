using Iter.Core.EntityModels;
using Iter.Core.EntityModelss;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class CitySeed : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> entity)
        {
            var data = Common.Deserialize<List<City>>("Cities.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
