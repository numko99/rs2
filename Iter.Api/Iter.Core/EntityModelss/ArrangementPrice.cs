namespace Iter.Core.EntityModels
{
    public class ArrangementPrice : BaseEntity
    {
        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement { get; set; }

        public string? AccommodationType { get; set; }

        public decimal Price { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
