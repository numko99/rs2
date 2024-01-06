using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;

namespace Iter.Services
{
    public class ReservationService : BaseCrudService<Reservation, ReservationUpsertRequest, ReservationUpsertRequest, ReservationResponse, AgencySearchModel>, IReservationService
    {
        public ReservationService(IReservationRepository reservationRepository, IMapper mapper) : base(reservationRepository, mapper)
        {
        }
    }
}