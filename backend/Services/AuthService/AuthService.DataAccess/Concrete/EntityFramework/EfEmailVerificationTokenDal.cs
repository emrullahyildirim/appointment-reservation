using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class EfEmailVerificationTokenDal : EfEntityRepositoryBase<EmailVerificationToken, AuthDbContext>, IEmailVerificationTokenDal
    {
        public EfEmailVerificationTokenDal(AuthDbContext context) : base(context)
        {
        }

        public EmailVerificationToken? GetByToken(string token)
        {
            return _context.EmailVerificationTokens
                .FirstOrDefault(t => t.Token == token && !t.IsUsed && t.ExpirationDate > DateTime.UtcNow);
        }

        public EmailVerificationToken? GetActiveTokenByUserId(int userId)
        {
            return _context.EmailVerificationTokens
                .FirstOrDefault(t => t.UserId == userId && !t.IsUsed && t.ExpirationDate > DateTime.UtcNow);
        }
    }
}

