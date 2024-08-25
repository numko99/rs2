namespace Iter.Model
{    public class DestinationResponse    {
        public Guid Id { get; set; }
        public string? City { get; set; }                public int? CityId { get; set; }        public string? Country { get; set; }        public int? CountryId { get; set; }        public AccommodationResponse Accommodation { get; set; }        public DateTime? ArrivalDate { get; set; }        public DateTime? DepartureDate { get; set; }        public bool IsOneDayTrip { get; set; }    }}