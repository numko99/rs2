using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core;
using Iter.Core.Search_Models;
using Iter.Infrastrucure;
using Iter.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Repository
{
    public class DestinationRepository : BaseCrudRepository<Destination, DestinationUpsertRequest, DestinationUpsertRequest, DestinationResponse, AgencySearchModel, DestinationResponse>, IDestinationRepository
    {
        public DestinationRepository(IterContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}