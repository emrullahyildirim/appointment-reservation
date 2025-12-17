using Core.DataAcces.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;

namespace PatientService.DataAccess.Concrete.EntityFramework
{
    public class EfDoctorDal : EfEntityRepositoryBase<Doctor, PatientAppointmentContext>, IDoctorDal
    {
        public List<Appointment> GetDoctorWithAppointments(int doctorId)
        {
            using (PatientAppointmentContext context = new PatientAppointmentContext())
            {
                var result = context.Appointments
                                    .Include(a => a.Patient)
                                    .Where(d => d.Id == doctorId)
                                    .Include(d => d.Doctor)
                                    .ToList();
                return result;
            }
        }
    }
}