using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Infrastrucure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Iter.Infrastrucure
{
    public class IterContext : IdentityDbContext<User>
    {
        public IterContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Accommodation> Accommodation { get; set; }

        public virtual DbSet<Address> Address { get; set; }

        public virtual DbSet<Agency> Agency { get; set; }

        public virtual DbSet<Arrangement> Arrangement { get; set; }

        public virtual DbSet<Destination> Destination { get; set; }

        public virtual DbSet<EmployeeArrangment> EmployeeArrangment { get; set; }

        public virtual DbSet<Reservation> Reservation { get; set; }

        public virtual DbSet<ReservationStatus> ReservationStatus { get; set; }

        public virtual DbSet<User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AccomataionConfiguration());
            builder.ApplyConfiguration(new AddressConfiguration());
            builder.ApplyConfiguration(new AgencyConfiguration());
            builder.ApplyConfiguration(new ArrangementConfiguration());
            builder.ApplyConfiguration(new DestinationConfiguration());
            builder.ApplyConfiguration(new EmployeeArrangmentConfiguration());
            builder.ApplyConfiguration(new ReservationConfiguration());
        }
    }
}
