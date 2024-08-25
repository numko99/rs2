using Iter.Core.EntityModelss;

namespace Iter.Repository.Interface
{
    public interface IVerificationTokenRepository : IBaseCrudRepository<VerificationToken>
    {
        Task<VerificationToken?> GetLastTokenByUserId(string userId);
    }
}
