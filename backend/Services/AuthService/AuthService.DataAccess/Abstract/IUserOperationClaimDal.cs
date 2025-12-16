using AuthService.Entities.Concrete;
using Core.DataAcces;

namespace AuthService.DataAccess.Abstract
{
    public interface IUserOperationClaimDal : IEntityRepository<UserOperationClaim>
    {
    }
}

