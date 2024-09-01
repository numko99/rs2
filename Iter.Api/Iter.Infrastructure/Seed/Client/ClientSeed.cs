using Iter.Core.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class ClientSeed : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            var data = Common.Deserialize<List<Client>>("Clients.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
