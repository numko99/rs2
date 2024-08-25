namespace Iter.Model
{    public class AccommodationResponse    {
        public Guid Id { get; set; }
        public string? HotelName { get; set; }        public AddressResponse HotelAddress { get; set; }        public DateTime? CheckInDate { get; set; }        public DateTime? CheckOutDate { get; set; }    }}