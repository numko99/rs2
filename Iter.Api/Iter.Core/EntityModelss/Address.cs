namespace Iter.Core.EntityModels
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public int? CityId { get; set; }

        public City? City { get; set; }

        public string PostalCode { get; set; }


        public List<Accommodation> Accommodations { get; set; }

        public List<Agency> Agencies { get; set; }
    }
}
