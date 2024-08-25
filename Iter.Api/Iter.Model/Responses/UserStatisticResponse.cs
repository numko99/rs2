namespace Iter.Model
{
    public class UserStatisticResponse
    {
        public int ReservationCount { get; set; }

        public decimal AvgRating { get; set; }

        public int ArrangementsCount { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
