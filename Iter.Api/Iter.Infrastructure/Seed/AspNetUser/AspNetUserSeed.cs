using Iter.Core.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class AspNetUserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            var data = Common.Deserialize<List<User>>("AspNetUsers.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
