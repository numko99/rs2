using Iter.Core.EntityModels;
using Iter.Infrastrucure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<ArrangementStatus> ArrangementStatus { get; set; }

        public virtual DbSet<ArrangementPrice> ArrangementPrice { get; set; }

        public virtual DbSet<ArrangementImage> ArrangementImage { get; set; }

        public virtual DbSet<Image> Image { get; set; }

        public virtual DbSet<Destination> Destination { get; set; }

        public virtual DbSet<EmployeeArrangment> EmployeeArrangment { get; set; }

        public virtual DbSet<Reservation> Reservation { get; set; }

        public virtual DbSet<ReservationStatus> ReservationStatus { get; set; }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Client> Client { get; set; }


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
            builder.ApplyConfiguration(new ArrangementPriceConfiguration());
            builder.ApplyConfiguration(new ArrangementImageConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new ClientConfiguration());
        }
    }
}
