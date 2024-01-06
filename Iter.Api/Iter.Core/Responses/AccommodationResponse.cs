using Iter.Core.Responses;

namespace Iter.Core.Requests
{    public class AccommodationResponse    {        public Guid Id { get; set; }        public string HotelName { get; set; }        public Guid HotelAddressId { get; set; }        public AddressResponse HotelAddress { get; set; }        public DateTime CheckInDate { get; set; }        public DateTime CheckOutDate { get; set; }        public bool IsDeleted { get; set; }        public DateTime DateCreated { get; set; }        public DateTime DateModified { get; set; }    }}