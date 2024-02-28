namespace Iter.Core.EntityModels
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }


        public List<Accommodation> Accommodations { get; set; }

        public List<Agency> Agencies { get; set; }

        public List<Employee> Employees { get; set; }

        public List<Client> Clients { get; set; }
    }
}
