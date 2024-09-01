using Iter.Core.EntityModels;
using Iter.Core.EntityModelss;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class AddressSeed : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            var data = Common.Deserialize<List<Address>>("Addresses.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
