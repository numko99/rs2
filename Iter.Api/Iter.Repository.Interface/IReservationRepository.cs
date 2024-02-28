using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;

namespace Iter.Repository.Interface
{
    public interface IReservationRepository : IBaseCrudRepository<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel>
    {
        Task<string> GetLatestReservationNumber();

        Task<int> GetCount(Guid arrangementId);
    }
}