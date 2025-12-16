using Core.DataAcces;
using Entities.Concrete;
using PatientService.Entities.Concrete;

namespace PatientService.DataAccess.Abstract
{
    public interface IDoctorDal : IEntityRepository<Doctor>
    {
         List<Appointment>  GetDoctorWithAppointments(int doctorId);
    }
}