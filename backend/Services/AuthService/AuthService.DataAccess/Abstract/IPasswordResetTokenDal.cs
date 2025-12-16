using AuthService.Entities.Concrete;
using Core.DataAcces;

namespace AuthService.DataAccess.Abstract
{
    public interface IPasswordResetTokenDal : IEntityRepository<PasswordResetToken>
    {
        PasswordResetToken? GetByToken(string token);
        PasswordResetToken? GetActiveTokenByUserId(int userId);
    }
}

