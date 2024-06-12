using Iter.Core.EntityModels;
using Iter.Core.Search_Models;
using Iter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iter.Core.EntityModelss;
using Iter.Repository.Interface;
using AutoMapper;
using Iter.Infrastrucure;
using Microsoft.EntityFrameworkCore;

namespace Iter.Repository
{
    public class VerificationTokenRepository : BaseCrudRepository<VerificationToken, ReservationInsertRequest, ReservationUpdateRequest, ReservationResponse, VerificationTokenSearchModel, ReservationSearchResponse>, IVerificationTokenRepository
    {
        private readonly IterContext dbContext;
        private readonly IMapper mapper;
        public VerificationTokenRepository(IterContext context, IMapper mapper) : base(context, mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        public async Task<VerificationToken?> GetLastTokenByUserId(string userId)
        {
            return await this.dbContext.VerificationToken.OrderBy(x => x.ExpiryDate).Where(x => x.UserId == userId).LastOrDefaultAsync();
        }
    }
}
