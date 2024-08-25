using Iter.Core.EntityModels;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class AccommodationRepository : BaseCrudRepository<Accommodation>, IAccommodationRepository
    {
        public AccommodationRepository(IterContext dbContext, ILogger<AccommodationRepository> logger) : base(dbContext, logger)
        {
        }
    }
}