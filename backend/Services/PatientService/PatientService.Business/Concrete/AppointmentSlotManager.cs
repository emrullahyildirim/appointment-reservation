using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class AppointmentSlotManager : IAppointmentSlotService
    {
        IAppointmentSlotDal _appointmentslotDal;

        public AppointmentSlotManager(IAppointmentSlotDal appointmentslotDal)
        {
            _appointmentslotDal = appointmentslotDal;
        }

        public IResult Add(AppointmentSlot appointmentslot)
        {
            _appointmentslotDal.Add(appointmentslot);
            return new SuccessResult(" eklendi");
        }

        public IResult Delete(AppointmentSlot appointmentslot)
        {
            _appointmentslotDal.Remove(appointmentslot);
            return new SuccessResult();
        }

        public IDataResult<List<AppointmentSlot>> GetAll()
        {
            return new SuccessDataResult<List<AppointmentSlot>>(_appointmentslotDal.GetAll(),"Message");
        }

        public IDataResult<List<AppointmentSlot>> GetAllByDoctorId(int id)
        {
            var result = _appointmentslotDal.GetAll(p => p.DoctorId == id);
            if (result.Count > 0)
            {
                return new SuccessDataResult<List<AppointmentSlot>>(result);
            }
            return new ErrorDataResult<List<AppointmentSlot>>();
        }

        public IDataResult<AppointmentSlot> GetById(int id)
        {
            return new SuccessDataResult<AppointmentSlot>(_appointmentslotDal.Get(p => p.Id == id));
        }

        public IResult Update(AppointmentSlot appointmentslot)
        {
            throw new NotImplementedException();
        }
    }
}

