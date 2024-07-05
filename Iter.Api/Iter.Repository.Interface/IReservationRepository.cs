using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IReservationRepository : IBaseCrudRepository<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>
    {
        Task<string> GetLatestReservationNumber();

        Task<int> GetCount(Guid arrangementId);

        Task UpdateRatingAsync(Guid reservationId, int? rating);

        Task<List<Reservation>> GetArrangementsByDestinationCityNames(List<int> cities);
    }
}