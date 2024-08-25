namespace Iter.Model
{
    public class AgencyInsertRequest
    {
        public string Name { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string Website { get; set; }

        public string LicenseNumber { get; set; }

        public ImageUpsertRequest? Logo { get; set; }

        public AddressInsertRequest Address { get; set; }
    }
}
