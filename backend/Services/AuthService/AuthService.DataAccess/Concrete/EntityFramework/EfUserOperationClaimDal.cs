using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, AuthDbContext>, IUserOperationClaimDal
    {
        public EfUserOperationClaimDal(AuthDbContext context) : base(context)
        {
        }
    }
}

