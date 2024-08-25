using Iter.Core.EntityModelss;

namespace Iter.Core.EntityModels
{
    public class Destination : BaseEntity
    {
        public int CityId { get; set; }

        public City City { get; set; }

        public Guid? AccommodationId { get; set; }

        public Accommodation? Accommodation { get; set; }

        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement{ get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public bool IsOneDayTrip { get; set; }

    }
}
