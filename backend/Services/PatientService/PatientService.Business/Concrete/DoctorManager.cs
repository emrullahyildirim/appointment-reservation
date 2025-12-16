using PatientService.Business.Abstract;
using Core.Utilities.Result;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;
using Entities.Concrete;

namespace PatientService.Business.Concrete
{
    public class DoctorManager : IDoctorService
    {
        IDoctorDal _doctorDal;

        public DoctorManager(IDoctorDal doctorDal)
        {
            _doctorDal = doctorDal;
        }

        public IResult Add(Doctor doctor)
        {
            _doctorDal.Add(doctor);
            return new SuccessResult(" eklendi");
        }

        public IResult Delete(Doctor doctor)
        {
            _doctorDal.Remove(doctor);
            return new SuccessResult();
        }

        public IDataResult<List<Doctor>> GetAll()
        {
            return new SuccessDataResult<List<Doctor>>(_doctorDal.GetAll(),"Message");
        }

        public IDataResult<List<Appointment>> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = _doctorDal.GetDoctorWithAppointments(doctorId);
            if (appointments.Count > 0)
            {
                return new SuccessDataResult<List<Appointment>>(appointments);
            }
            return new ErrorDataResult<List<Appointment>>();

        }

        public IDataResult<Doctor> GetById(int id)
        {
            return new SuccessDataResult<Doctor>(_doctorDal.Get(p => p.Id == id));
        }

        public IResult Update(Doctor doctor)
        {
            throw new NotImplementedException();
        }
    }
}

