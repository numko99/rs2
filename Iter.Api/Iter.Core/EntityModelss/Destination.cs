namespace Iter.Core.EntityModels
{
    public class Destination : BaseEntity
    {
        public string City { get; set; }

        public string Country { get; set; }

        public Guid? AccommodationId { get; set; }

        public Accommodation? Accommodation { get; set; }

        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement{ get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public bool IsOneDayTrip { get; set; }

    }
}
