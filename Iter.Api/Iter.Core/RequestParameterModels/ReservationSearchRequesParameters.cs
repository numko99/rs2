namespace Iter.Core.RequestParameterModels
{
    public class ReservationSearchRequesParameters : BaseSearchModel
    {
        public string? Name { get; set; }

        public string? AgencyId { get; set; }

        public string? UserId { get; set; }

        public string? ArrangementId { get; set; }

        public string? ReservationStatusId { get; set; }

        public bool? ReturnActiveReservations { get; set; }
    }
}
