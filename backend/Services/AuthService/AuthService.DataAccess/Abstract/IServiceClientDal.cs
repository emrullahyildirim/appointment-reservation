using AuthService.Entities.Concrete;
using Core.DataAcces;

namespace AuthService.DataAccess.Abstract
{
    public interface IServiceClientDal : IEntityRepository<ServiceClient>
    {
        ServiceClient? GetByClientId(string clientId);
        ServiceClient? GetByServiceName(string serviceName);
    }
}
