using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Core.Models;
using Iter.Core.RequestParameterModels;

namespace Iter.Repository.Interface
{
    public interface IReservationRepository : IBaseCrudRepository<Reservation>
    {
        Task<string> GetLatestReservationNumber();

        Task<int> GetCount(Guid arrangementId);

        Task UpdateRatingAsync(Guid reservationId, int? rating);

        Task<List<Reservation>> GetArrangementsByDestinationCityNames(List<int> cities);

        Task<PagedResult<ReservationSearchDto>> Get(ReservationSearchRequesParameters? search);

        Task<int> GetCount(Guid? agencyId = null);

        Task<decimal> GetTotalAmount(Guid? agencyId = null);
    }
}