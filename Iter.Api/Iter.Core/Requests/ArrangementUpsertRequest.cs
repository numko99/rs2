namespace Iter.Core.Requests
{    public class ArrangementUpsertRequest    {
        //public Guid Id { get; set; }

        //public Guid AgencyId { get; set; }

        public string Name { get; set; }

        public string StartDate { get; set; }        public string EndDate { get; set; }

        public decimal? Price { get; set; }

        public List<ArrangmentPriceUpsertRequest> Prices { get; set; }

        public string? Description { get; set; }

        public List<ImageUpsertRequest> Images { get; set; }

        public ImageUpsertRequest? MainImage { get; set; }

        public List<DestinationUpsertRequest> Destinations { get; set; }
    }}