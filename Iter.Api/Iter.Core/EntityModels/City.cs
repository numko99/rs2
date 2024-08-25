using Iter.Core.EntityModels;

namespace Iter.Core.EntityModelss
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public List<Destination> Destinations { get; set; }

        public List<Address> Address { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
