using Core.DataAcces;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IAppointmentDal : IEntityRepository<Appointment>
    {
    }
}