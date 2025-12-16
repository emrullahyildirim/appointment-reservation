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
                var result = context.Doctors.Include(d => d.Appointments)
                                            .Where(d => d.Id == doctorId)
                                            .SelectMany(d => d.Appointments)
                                            .ToList();
                return result;
            }
        }
    }
}