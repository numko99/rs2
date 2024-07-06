using Iter.Core.EntityModels;
using Iter.Core.Search_Models;
using Iter.Core;
using Iter.Core.EntityModelss;

namespace Iter.Repository.Interface
{
    public interface IVerificationTokenRepository : IBaseCrudRepository<VerificationToken>
    {
        Task<VerificationToken?> GetLastTokenByUserId(string userId);
    }
}
