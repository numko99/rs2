namespace Iter.Core.Models
{
    public class Arrangement
    {
        public Guid Id { get; set; }

        public Guid AgencyId { get; set; }

        public Agency Agency { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }



        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }



        public List<Reservation> Reservations { get; set; }

        public List<Destination> Destinations { get; set; }

        public List<EmployeeArrangment> EmployeeArrangments{ get; set; }
    }
}
