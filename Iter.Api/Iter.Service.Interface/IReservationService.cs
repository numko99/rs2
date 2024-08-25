using Iter.Core.EntityModels;
using Iter.Model;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IReservationService : IBaseCrudService<Reservation, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, ReservationSearchModel, ReservationSearchResponse>
    {
        Task<int> GetCount(Guid arrangementId);

        Task AddReview(Guid reservationId, int? rating);

        Task AddPayment(Guid reservationId, int totalPaid, string transactionId);

        Task CancelReservation(Guid reservationId);

        Task<ReservationResponse> Insert(ReservationInsertRequest request);
    }
}