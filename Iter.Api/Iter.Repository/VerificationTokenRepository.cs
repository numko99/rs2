using Iter.Core.EntityModelss;
using Iter.Repository.Interface;
using Iter.Infrastrucure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Iter.Repository
{
    public class VerificationTokenRepository : BaseCrudRepository<VerificationToken>, IVerificationTokenRepository
    {
        private readonly IterContext dbContext;
        private readonly ILogger<VerificationTokenRepository> logger;

        public VerificationTokenRepository(IterContext context, ILogger<VerificationTokenRepository> logger) : base(context, logger)
        {
            this.dbContext = context;
            this.logger = logger;
        }

        public async Task<VerificationToken?> GetLastTokenByUserId(string userId)
        {
            logger.LogInformation("GetLastTokenByUserId operation started for User ID: {UserId}", userId);

            try
            {
                var token = await dbContext.VerificationToken
                    .OrderBy(x => x.ExpiryDate)
                    .Where(x => x.UserId == userId)
                    .LastOrDefaultAsync();

                if (token == null)
                {
                    logger.LogWarning("No verification token found for User ID: {UserId}", userId);
                }
                else
                {
                    logger.LogInformation("GetLastTokenByUserId operation completed successfully for User ID: {UserId}", userId);
                }

                return token;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during GetLastTokenByUserId operation for User ID: {UserId}", userId);
                throw;
            }
        }
    }
}
