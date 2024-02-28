namespace Iter.Core.EntityModels
{
    public class Accommodation : BaseEntity
    {
        public string HotelName { get; set; }

        public Guid HotelAddressId { get; set; }

        public Address HotelAddress { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }


        public List<Destination> Destinations{ get; set; }

    }
}
