namespace Iter.Core.Responses
{
    public class AddressResponse
    {
        public Guid Id { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
