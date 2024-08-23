namespace Iter.Core
{    public class ReservationUpdateRequest    {        public string? DeparturePlace { get; set; }

        public int DepartureCityId { get; set; }        public string? Reminder { get; set; }                public string? ArrangementPriceId { get; set; }                public string? ReservationStatusId{ get; set; }        public int? Rating { get; set; }        public string? RatingComment { get; set; }        public decimal? TotalPaid { get; set; }        public Guid? TransactionId { get; set; }    }}