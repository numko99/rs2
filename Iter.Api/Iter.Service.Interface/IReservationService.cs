using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;
using Iter.Services.Interfaces;

namespace Iter.Services.Interface
{
    public interface IReservationService : IBaseCrudService<Reservation, ReservationUpsertRequest, ReservationUpsertRequest, ReservationResponse, AgencySearchModel>
    {
    }
}