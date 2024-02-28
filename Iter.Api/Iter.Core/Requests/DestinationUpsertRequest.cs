namespace Iter.Core
{    public class DestinationUpsertRequest    {
        public string? Id { get; set; }

        public string? City { get; set; }        public string? Country { get; set; }        public AccommodationUpsertRequest? Accommodation { get; set; }        public DateTime? ArrivalDate { get; set; }        public DateTime? DepartureDate { get; set; }        public bool IsOneDayTrip { get; set; }    }}