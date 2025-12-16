using AuthService.Entities.Concrete;
using Core.DataAcces;

namespace AuthService.DataAccess.Abstract
{
    public interface IOperationClaimDal : IEntityRepository<OperationClaim>
    {
        OperationClaim? GetByName(string name);
    }
}

