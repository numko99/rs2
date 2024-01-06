using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;

namespace Iter.Repository
{
    public class ReservationRepository : BaseCrudRepository<Reservation, ReservationUpsertRequest, ReservationUpsertRequest, ReservationResponse, AgencySearchModel>, IReservationRepository
    {
        public ReservationRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}