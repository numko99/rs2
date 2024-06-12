namespace Iter.Core.EntityModels
{
    public class Arrangement : BaseEntity
    {
        public Guid AgencyId { get; set; }

        public Agency Agency { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ArrangementStatusId { get; set; }

        public decimal Rating { get; set; }

        public ArrangementStatus? ArrangementStatus { get; set; }

        public List<Reservation> Reservations { get; set; }

        public List<Destination> Destinations { get; set; }

        public List<ArrangementPrice> ArrangementPrices { get; set; }

        public List<ArrangementImage> ArrangementImages { get; set; }

        public List<EmployeeArrangment> EmployeeArrangments{ get; set; }
    }
}
