using Iter.Core.EntityModels;

namespace Iter.Core
{    public class ReservationResponse    {        public Guid Id { get; set; }        public string ReservationNumber { get; set; }        public string DeparturePlace { get; set; }        public string Reminder { get; set; }

        public UserResponse User { get; set; }        public ArrangementResponse Arrangement { get; set; }        public ArrangementPriceResponse ArrangementPrice { get; set; }        public Guid? ArrangementPriceId { get; set; }        public int ReservationStatusId { get; set; }        public string? ReservationStatusName { get; set; }        public decimal TotalPaid { get; set; }        public int Rating { get; set; }    }}