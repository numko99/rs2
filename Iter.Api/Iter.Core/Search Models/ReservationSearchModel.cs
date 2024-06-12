namespace Iter.Core.Search_Models
{
    public class ReservationSearchModel: BaseSearchModel
    {
        public string? Name { get; set; }

        public string? AgencyId { get; set; }

        public string? UserId { get; set; }

        public string? ArrangementId { get; set; }

        public string? ReservationStatusId { get; set; }

        public bool? ReturnActiveReservations { get; set; }
    }
}
