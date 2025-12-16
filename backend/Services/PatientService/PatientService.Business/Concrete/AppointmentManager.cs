using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using PatientService.Entities.DTOs.Appointment;
using PatientService.Entities.Enums;

namespace Business.Concrete
{
    public class AppointmentManager : IAppointmentService
    {
        IAppointmentDal _appointmentDal;

        public AppointmentManager(IAppointmentDal appointmentDal)
        {
            _appointmentDal = appointmentDal;
        }

        public IResult Add(Appointment appointment)
        {
            appointment.Status = AppointmentStatus.Pending;

            _appointmentDal.Add(appointment);
            return new SuccessResult(" eklendi");
        }

        public IResult Delete(Appointment appointment)
        {
            _appointmentDal.Remove(appointment);
            return new SuccessResult();
        }

        public IDataResult<List<Appointment>> GetAll()
        {
            return new SuccessDataResult<List<Appointment>>(_appointmentDal.GetAll(),"Message");
        }

        public IDataResult<Appointment> GetById(int id)
        {
            return new SuccessDataResult<Appointment>(_appointmentDal.Get(p => p.Id == id));
        }

        public IDataResult<List<Appointment>> GetPacientHistories(int patientId)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var result = _appointmentDal.GetAll(p => p.PatientId == patientId && p.AppointmentSlot.SlotDate < today);
            if (result.Count > 0)
            {
                return new SuccessDataResult<List<Appointment>>(result);
            }
            return new ErrorDataResult<List<Appointment>>();
        }

        public IResult Update(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}

