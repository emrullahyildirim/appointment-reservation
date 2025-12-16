using AuthService.DataAccess.Abstract;
using AuthService.Entities.Concrete;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    public class EfServiceClientDal : EfEntityRepositoryBase<ServiceClient, AuthDbContext>, IServiceClientDal
    {
        public EfServiceClientDal(AuthDbContext context) : base(context)
        {
        }

        public ServiceClient? GetByClientId(string clientId)
        {
            return _context.ServiceClients.FirstOrDefault(sc => sc.ClientId == clientId);
        }

        public ServiceClient? GetByServiceName(string serviceName)
        {
            return _context.ServiceClients.FirstOrDefault(sc => sc.ServiceName == serviceName);
        }
    }
}
