namespace Iter.Core
{    public class ReservationUpdateRequest    {        public string? DeparturePlace { get; set; }        public string Reminder { get; set; }                public string? ArrangementPriceId { get; set; }                public string? ReservationStatusId{ get; set; }        public decimal? TotalPaid { get; set; }    }}