using Iter.Core.EntityModels;
using Iter.Core.EntityModelss;
using Iter.Core.Models;
using Iter.Infrastructure;
using Iter.Infrastrucure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Iter.Infrastrucure
{
    public class IterContext : IdentityDbContext<User>
    {
        public void Initialize()
        {
            if (Database.GetPendingMigrations()?.Count() > 0)
            {
                Database.Migrate();


                var assembly = typeof(IterContext).Assembly;
                var assemblyName = assembly.FullName[..assembly.FullName.IndexOf(',')];
                using var resource = assembly.GetManifestResourceStream($"{assemblyName}.iter_seed.sql");

                using var streamReader = new StreamReader(resource, encoding: Encoding.Unicode);
                var sqlScript = streamReader.ReadToEnd();

                // Podijeli skriptu na pojedinačne naredbe na temelju 'GO'
                var sqlCommands = sqlScript.Split("GO\r", StringSplitOptions.RemoveEmptyEntries);

                this.Database.SetCommandTimeout(600);
                foreach (var command in sqlCommands)
                {
                    if (!string.IsNullOrWhiteSpace(command))
                    {
                        try
                        {
                            this.Database.ExecuteSqlRaw(command);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }

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

        public virtual DbSet<VerificationToken> VerificationToken { get; set; }

        public virtual DbSet<City> City { get; set; }

        public virtual DbSet<Country> Country { get; set; }


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
            builder.ApplyConfiguration(new VerificationTokenConfiguration());
            builder.ApplyConfiguration(new CityConfiguration());
            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new ArrangementStatusConfiguration());
            builder.ApplyConfiguration(new ReservationStatusConfiguration());

            builder.ApplyConfiguration(new CountrySeed());
            builder.ApplyConfiguration(new CitySeed());
            builder.ApplyConfiguration(new AddressSeed());
            builder.ApplyConfiguration(new ReservationStatusSeed());
            builder.ApplyConfiguration(new AgencySeed());
            builder.ApplyConfiguration(new EmployeeSeed());
            builder.ApplyConfiguration(new ClientSeed());
            builder.ApplyConfiguration(new ArrangementStatusSeed());
            builder.ApplyConfiguration(new AspNetRoleSeed());
            builder.ApplyConfiguration(new AspNetUserSeed());
            builder.ApplyConfiguration(new AspNetUserRoleSeed());
        }
    }
}
