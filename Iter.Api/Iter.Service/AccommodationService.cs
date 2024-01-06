using AutoMapper;
using Iter.Core.EntityModels;
using Iter.Core.Requests;
using Iter.Core.Responses;
using Iter.Core.Search_Models;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter.Services
{
    public class AccommodationService : BaseCrudService<Accommodation, AccommodationUpsertRequest, AccommodationUpsertRequest, AccommodationResponse, AgencySearchModel>, IAccommodationService
    {
        private readonly IAccommodationRepository accommodationRepository;
        private readonly IMapper mapper;
        public AccommodationService(IAccommodationRepository accommodationRepository, IMapper mapper) : base(accommodationRepository, mapper)
        {
            this.accommodationRepository = accommodationRepository;
            this.mapper = mapper;
        }
    }
}