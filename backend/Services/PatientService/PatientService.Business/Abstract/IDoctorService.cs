using Core.Utilities.Result;
using Entities.Concrete;
using PatientService.Entities.Concrete;

namespace PatientService.Business.Abstract
{
    public interface IDoctorService
    {
        IResult Add(Doctor doctor);
        IResult Update(Doctor doctor);
        IResult Delete(Doctor doctor);
        IDataResult<Doctor> GetById(int id);
        IDataResult<List<Appointment>> GetAppointmentsByDoctorId(int doctorId);
        IDataResult<List<Doctor>> GetAll();
    }
}

