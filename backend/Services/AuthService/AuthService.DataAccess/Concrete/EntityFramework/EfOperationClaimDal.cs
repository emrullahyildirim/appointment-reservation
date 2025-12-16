using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, AuthDbContext>, IOperationClaimDal
    {
        public EfOperationClaimDal(AuthDbContext context) : base(context)
        {
        }

        public OperationClaim? GetByName(string name)
        {
            return _context.OperationClaims.FirstOrDefault(oc => oc.Name == name);
        }
    }
}

