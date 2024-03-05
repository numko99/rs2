namespace Iter.Core
{    public class ArrangementResponse    {
        public Guid Id { get; set; }

        public AgencyResponse Agency { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }        public DateTime? EndDate { get; set; }

        public decimal? Price { get; set; }

        public List<ArrangementPriceResponse> Prices { get; set; }

        public string? Description { get; set; }

        public int ArrangementStatusId { get; set; }

        public string ArrangementStatusName { get; set; }

        public List<ImageResponse> Images { get; set; }

        public ImageResponse? MainImage { get; set; }

        public List<DestinationResponse> Destinations { get; set; }
    }}