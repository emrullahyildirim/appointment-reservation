using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class EfPasswordResetTokenDal : EfEntityRepositoryBase<PasswordResetToken, AuthDbContext>, IPasswordResetTokenDal
    {
        public EfPasswordResetTokenDal(AuthDbContext context) : base(context)
        {
        }

        public PasswordResetToken? GetByToken(string token)
        {
            return _context.PasswordResetTokens
                .FirstOrDefault(t => t.Token == token && !t.IsUsed && t.ExpirationDate > DateTime.UtcNow);
        }

        public PasswordResetToken? GetActiveTokenByUserId(int userId)
        {
            return _context.PasswordResetTokens
                .FirstOrDefault(t => t.UserId == userId && !t.IsUsed && t.ExpirationDate > DateTime.UtcNow);
        }
    }
}

