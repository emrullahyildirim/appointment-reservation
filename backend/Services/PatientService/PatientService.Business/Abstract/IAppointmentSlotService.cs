using Core.Utilities.Result;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IAppointmentSlotService
    {
        IResult Add(AppointmentSlot appointmentslot);
        IResult Update(AppointmentSlot appointmentslot);
        IResult Delete(AppointmentSlot appointmentslot);
        IDataResult<AppointmentSlot> GetById(int id);
        IDataResult<List<AppointmentSlot>> GetAll();
        IDataResult<List<AppointmentSlot>> GetAllByDoctorId(int id);

    }
}

