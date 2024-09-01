﻿using Iter.Core.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class AspNetRoleSeed : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> entity)
        {
            var data = Common.Deserialize<List<IdentityRole<string>>>("AspNetRoles.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}