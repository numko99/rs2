namespace Iter.Core.EntityModels
{
    public class Reservation : BaseEntity
    {
        public string ReservationNumber { get; set; }

        public string DeparturePlace { get; set; }

        public string Reminder { get; set; }
        
        public Guid ClientId { get; set; }
        
        public Client Client { get; set; }

        public Guid ArrangmentId { get; set; }

        public Arrangement Arrangement { get; set; }

        public int ReservationStatusId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }

        public Guid ArrangementPriceId { get; set; }

        public ArrangementPrice ArrangementPrice { get; set; }

        public decimal TotalPaid { get; set; }

        public int Rating { get; set; }
    }
}
