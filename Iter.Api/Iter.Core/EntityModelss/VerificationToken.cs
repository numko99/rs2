using Iter.Core.EntityModels;

namespace Iter.Core.EntityModelss
{
    public class VerificationToken
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User? User { get; set; }

        public string Token { get; set; }

        public string VerificationTokenType { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
