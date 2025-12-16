using Core.Utilities.Result;
using Entities.Concrete;
using PatientService.Entities.DTOs.Appointment;

namespace Business.Abstract
{
    public interface IAppointmentService
    {
        IResult Add(Appointment Appointment);
        IResult Update(Appointment appointment);
        IResult Delete(Appointment appointment);
        IDataResult<Appointment> GetById(int id);
        IDataResult<List<Appointment>> GetAll();
        IDataResult<List<Appointment>> GetPacientHistories(int patientId);
    }
}

