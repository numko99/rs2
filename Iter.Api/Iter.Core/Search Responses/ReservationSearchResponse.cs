namespace Iter.Core
{
    public class ReservationSearchResponse
    {
        public Guid ReservationId { get; set; }

        public ImageResponse MainImage { get; set; }

        public string ArrangementName { get; set; }

        public Guid ArrangementId { get; set; }

        public DateTime ArrangementStartDate { get; set; }

        public DateTime? ArrangementStartEndDate { get; set; }

        public int ReservationStatusId { get; set; }

        public string ReservationStatusName { get; set; }

        public string AgencyName { get; set; }

        public decimal ArrangementPrice { get; set; }

        public decimal TotalPaid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime ReservationDate { get; set; }

        public string ReservationNumber { get; set; }
    }
}
