namespace Iter.Model
{
    public class AdminStatisticResponse
    {
        public int ReservationCount { get; set; }

        public int ArrangementCount { get; set; }

        public int UsersCount { get; set; }

        public decimal TotalAmount { get; set; }

        public int AgenciesCount { get; set; }

        public List<ReservationDiagramResponse> Reservations { get; set; }
    }
}
