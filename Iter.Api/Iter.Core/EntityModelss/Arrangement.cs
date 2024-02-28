namespace Iter.Core.EntityModels
{
    public class Arrangement : BaseEntity
    {
        public Guid AgencyId { get; set; }

        public Agency Agency { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }


        public List<Reservation> Reservations { get; set; }

        public List<Destination> Destinations { get; set; }

        public List<ArrangementPrice> ArrangementPrices { get; set; }

        public List<ArrangementImage> ArrangementImages { get; set; }

        public List<EmployeeArrangment> EmployeeArrangments{ get; set; }
    }
}
