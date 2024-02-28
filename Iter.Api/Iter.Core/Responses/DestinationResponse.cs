namespace Iter.Core
{    public class DestinationResponse    {
        public Guid Id { get; set; }
        public string? City { get; set; }        public string? Country { get; set; }        public AccommodationResponse Accommodation { get; set; }        public DateTime? ArrivalDate { get; set; }        public DateTime? DepartureDate { get; set; }        public bool IsOneDayTrip { get; set; }    }}