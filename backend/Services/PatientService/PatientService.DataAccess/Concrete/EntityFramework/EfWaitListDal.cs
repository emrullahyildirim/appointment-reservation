using Core.DataAcces.EntityFramework;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;
using PatientService.Entities.Enums;

namespace PatientService.DataAccess.Concrete.EntityFramework
{
    public class EfWaitlistDal : EfEntityRepositoryBase<Waitlist, PatientAppointmentContext>, IWaitlistDal
    {
        public Waitlist GetNextInQueue()
        {
            using (PatientAppointmentContext context = new PatientAppointmentContext())
            {
                var nextInQueue = context.Waitlists
                    .Where(w => w.Status == WaitlistStatus.Waiting)
                    .OrderBy(w => w.CreatedAt)
                    .FirstOrDefault();
                return nextInQueue;
            }
        }

        public int GetNumberOfWaitlistInQueue()
        {
            using (PatientAppointmentContext context = new PatientAppointmentContext())
            {
                var count = context.Waitlists.Count(w => w.Status == WaitlistStatus.Waiting);
                return count;
            }
        }
    }
}