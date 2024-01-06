namespace Iter.Core.EntityModels
{
    public class Accommodation
    {
        public Guid Id { get; set; }

        public string HotelName { get; set; }

        public Guid HotelAddressId { get; set; }

        public Address HotelAddress { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }


        public List<Destination> Destinations{ get; set; }

    }
}
