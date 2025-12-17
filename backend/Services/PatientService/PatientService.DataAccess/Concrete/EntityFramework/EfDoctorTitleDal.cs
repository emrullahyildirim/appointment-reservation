using Core.DataAcces.EntityFramework;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;

namespace PatientService.DataAccess.Concrete.EntityFramework
{
     public class EfDoctorTitleDal : EfEntityRepositoryBase<DoctorTitle, PatientAppointmentContext>, IDoctorTitleDal
    {
    }
}