namespace Iter.Core
{
    public class ArrangementSearchResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AgencyName { get; set; }

        public decimal AgencyRating { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ArrangementStatusId { get; set; }

        public string ArrangementStatusName { get; set; }

        public ImageResponse? MainImage { get; set; }

        public bool IsReserved { get; set; }
    }
}
