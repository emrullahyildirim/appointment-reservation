using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, AuthDbContext>, IUserDal
    {
        public EfUserDal(AuthDbContext context) : base(context)
        {
        }

        public List<OperationClaim> GetClaims(User user)
        {
            var result = from operationClaim in _context.OperationClaims
                         join userOperationClaim in _context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                         where userOperationClaim.UserId == user.Id
                         select new OperationClaim
                         {
                             Id = operationClaim.Id,
                             Name = operationClaim.Name,
                             Description = operationClaim.Description
                         };

            return result.ToList();
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetByRefreshToken(string refreshToken)
        {
            return _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
        }
    }
}

