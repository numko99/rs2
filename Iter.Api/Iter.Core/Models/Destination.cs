namespace Iter.Core.Models
{
    public class Destination
    {
        public Guid Id { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public Guid AccommodationId { get; set; }

        public Accommodation Accommodation { get; set; }

        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement{ get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public bool IsOneDayTrip { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
