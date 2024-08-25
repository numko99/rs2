using Iter.Core.EntityModels;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class DestinationRepository : BaseCrudRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(IterContext dbContext, ILogger<DestinationRepository> logger) : base(dbContext, logger)
        {
        }
    }
}