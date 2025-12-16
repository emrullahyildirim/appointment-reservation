using Core.DataAcces;
using PatientService.Entities.Concrete;

namespace PatientService.DataAccess.Abstract
{
    public interface IWaitlistDal : IEntityRepository<Waitlist>
    {
        int GetNumberOfWaitlistInQueue();
        Waitlist GetNextInQueue();
    }
}