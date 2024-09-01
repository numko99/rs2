using Iter.Core.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iter.Infrastructure
{
    public class EmployeeSeed : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            var data = Common.Deserialize<List<Employee>>("Employees.json");
            if (data != null)
            {
                entity.HasData(data);
            }
        }
    }
}
