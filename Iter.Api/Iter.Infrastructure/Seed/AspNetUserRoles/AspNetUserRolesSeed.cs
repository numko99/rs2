using Iter.Core.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class AspNetUserRoleSeed : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> entity)
        {
            var data = Common.Deserialize<List<IdentityUserRole<string>>>("AspNetUserRoles.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}