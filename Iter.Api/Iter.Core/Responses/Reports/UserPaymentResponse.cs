namespace Iter.Core.Responses
{
    public class UserPaymentResponse
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal? TotalPaid { get; set; }

        public string? ReservationNumber { get; set; }

        public string? ArrangementId { get; set; }

        public string? ArrangementName { get; set; }

        public string? TransactionId { get; set; }

    }
}
