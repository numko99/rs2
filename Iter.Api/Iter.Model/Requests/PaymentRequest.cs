namespace Iter.Model
{
    public class PaymentRequest
    {
        public string? ReservationId { get; set; }
        
        public int TotalPaid { get; set; }

        public string? TransactionId { get; set; }
    }
}
