namespace Iter.Core.Dto
{
    public class ArrangementSearchDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AgencyName { get; set; }

        public decimal AgencyRating { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ArrangementStatusId { get; set; }

        public string ArrangementStatusName { get; set; }

        public ImageDto? MainImage { get; set; }

        public bool IsReserved { get; set; }

        public decimal Rating { get; set; }

        public decimal MinPrice { get; set; }
    }
}
