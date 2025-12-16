using PatientService.Business.Abstract;
using Core.Utilities.Result;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;
using PatientService.Entities.Enums;
using Business.Abstract;
using Core.Utilities.Notification.Mail;

namespace PatientService.Business.Concrete
{
    public class WaitlistManager : IWaitlistService
    {
        IWaitlistDal _waitlistDal;
        private readonly IAppointmentSlotService _appointmentSlotService;
        private readonly IMailService _mailService;
        public WaitlistManager(IWaitlistDal waitlistDal, IAppointmentSlotService appointmentSlotService, IMailService mailService)
        {
            _waitlistDal = waitlistDal;
            _appointmentSlotService = appointmentSlotService;
            _mailService = mailService;
        }

        public IResult Add(Waitlist waitlist)
        {
            var isThereWaitlist = _waitlistDal.Get(w => w.PatientId == waitlist.PatientId);
            if (isThereWaitlist != null)
            {
                return new ErrorResult("Daha önce bir bekleme listesine girdiniz.");
            }
            var queueIndex = _waitlistDal.GetNumberOfWaitlistInQueue() + 1;
            waitlist.Status = WaitlistStatus.Waiting;
            _waitlistDal.Add(waitlist);
            return new SuccessResult(" eklendi");
        }

        public IResult Delete(Waitlist waitlist)
        {
            _waitlistDal.Remove(waitlist);
            return new SuccessResult();
        }

        public IDataResult<List<Waitlist>> GetAll()
        {
            return new SuccessDataResult<List<Waitlist>>(_waitlistDal.GetAll(),"Message");
        }

        public IDataResult<Waitlist> GetById(int id)
        {
            return new SuccessDataResult<Waitlist>(_waitlistDal.Get(p => p.Id == id));
        }

        public IResult Update(Waitlist waitlist)
        {
            throw new NotImplementedException();
        }

        public IResult CancelWaitlist(int waitlistId)
        {
            var waitlistEntity = _waitlistDal.Get(w => w.Id == waitlistId);
            waitlistEntity.Status = WaitlistStatus.Cancelled;

            return new SuccessResult();
        }

        public IDataResult<Waitlist> GetWaitlistByPatientId(int patientId)
        {
            var result = _waitlistDal.Get(w => w.PatientId == patientId);
            if (result != null)
            {
                return new SuccessDataResult<Waitlist>(result);
            }
            return new ErrorDataResult<Waitlist>("Bekleme listesi bulunamadý.");
        }

        public Task<IResult> NotifyNextInQueue()
        {
            var nextInQueue = _waitlistDal.GetNextInQueue();
            if (nextInQueue == null)
            {
                return Task.FromResult<IResult>(new ErrorResult("Bekleme listesinde kimse yok."));
            }
            //_mailService.SendMessageAsync(nextInQueue.PatientEmail, "Randevu Slotu Mevcut", "Size uygun bir randevu slotu açýldý. Lütfen randevunuzu alýn.");
            return Task.FromResult<IResult>(new SuccessResult("Bildirim gönderildi."));
        }
    }
}

