namespace Iter.Core.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public string ReservationNumber { get; set; }

        public string DeparturePlace { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }

        public Guid ArrangmentId { get; set; }

        public Arrangement Arrangement { get; set; }

        public int StatusId { get; set; }

        public ReservationStatus Status { get; set; }

        public decimal TotalPaid { get; set; }

        public int Rating { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

    }
}
