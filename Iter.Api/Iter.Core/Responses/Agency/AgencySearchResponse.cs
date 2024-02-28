namespace Iter.Core
{
    public class AgencySearchResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? City { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public int TotalCount { get; set; }
    }
}
