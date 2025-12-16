using AuthService.Entities.Concrete;
using Core.DataAcces;

namespace AuthService.DataAccess.Abstract
{
    public interface IEmailVerificationTokenDal : IEntityRepository<EmailVerificationToken>
    {
        EmailVerificationToken? GetByToken(string token);
        EmailVerificationToken? GetActiveTokenByUserId(int userId);
    }
}

