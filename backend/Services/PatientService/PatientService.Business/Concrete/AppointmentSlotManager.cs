using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using PatientService.Business.Abstract;
using PatientService.Entities.Concrete;
using PatientService.Entities.Enums;

namespace Business.Concrete
{
    public class AppointmentSlotManager : IAppointmentSlotService
    {
        IAppointmentSlotDal _appointmentslotDal;
        private readonly IDoctorService _doctorService;

        public AppointmentSlotManager(IAppointmentSlotDal appointmentslotDal, IDoctorService doctorService)
        {
            _appointmentslotDal = appointmentslotDal;
            _doctorService = doctorService;
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
            return new SuccessDataResult<List<AppointmentSlot>>(_appointmentslotDal.GetAll(includes: a => a.Doctor),"Message");
        }

        public IDataResult<List<AppointmentSlot>> GetAllByDoctorId(int id)
        {
            var result = _appointmentslotDal.GetAll(p => p.DoctorId == id, includes: a => a.Doctor);
            if (result.Count > 0)
            {
                return new SuccessDataResult<List<AppointmentSlot>>(result);
            }
            return new ErrorDataResult<List<AppointmentSlot>>();
        }

        public IDataResult<List<AppointmentSlot>> GetAvailableSlot(DateOnly preferredDate, TimeOnly? preferredStartTime, TimeOnly? preferredEndTime)
        {
            throw new NotImplementedException();
        }

        public IDataResult<AppointmentSlot> GetById(int id)
        {
            return new SuccessDataResult<AppointmentSlot>(_appointmentslotDal.Get(p => p.Id == id));
        }

        public IResult Update(AppointmentSlot appointmentslot)
        {
            throw new NotImplementedException();
        }



        public async Task<IResult> GenerateFutureSlotsAsync()
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(10);

            var doctorsResult = await _doctorService.GetAllAsyncAsNoTracking();
            if (!doctorsResult.IsSuccess || doctorsResult.Data == null)
                return new ErrorResult("Doctors could not be loaded");

            foreach (var doctor in doctorsResult.Data)
            {
                for (var day = startDate; day <= endDate; day = day.AddDays(1))
                {

                    var slots = CreateDailySlots(doctor, day);

                    foreach (var slot in slots)
                    {
                        var exists = await _appointmentslotDal
                            .AnyAsync(s => s.DoctorId == doctor.Id &&
                                      s.StartTime == slot.StartTime);

                        if (exists)
                            continue;

                        _appointmentslotDal.Add(slot);
                    }
                }
            }
            return new SuccessResult("Slots generated successfully");
        }



        public static List<AppointmentSlot> CreateDailySlots(Doctor doctor, DateTime day)
        {
            var slots = new List<AppointmentSlot>();

            var startDay = day.Date.AddHours(9);
            var endDay = day.Date.AddHours(17);

            TimeOnly startTime = TimeOnly.FromDateTime(startDay);
            TimeOnly endTime = TimeOnly.FromDateTime(endDay);

            var today = DateOnly.FromDateTime(day);
            while (startTime < endTime)
            {
                slots.Add(new AppointmentSlot
                {
                    DoctorId = doctor.Id,
                    Status = AppointmentSlotStatus.Available,
                    SlotDate = today,
                    StartTime = startTime,
                    EndTime = startTime.AddMinutes(20)
                });

                startTime = startTime.AddMinutes(20);
            }
            return slots;
        }

    }
}
