using Core.Utilities.Result;
using PatientService.Entities.Concrete;

namespace PatientService.Business.Abstract
{
    public interface IWaitlistService
    {
        IResult Add(Waitlist waitlist);
        IResult Update(Waitlist waitlist);
        IResult Delete(Waitlist waitlist);
        IDataResult<Waitlist> GetById(int id);
        IDataResult<Waitlist> GetWaitlistByPatientId(int patientId);
        IResult CancelWaitlist(int waitlistId);
        Task<IResult> NotifyNextInQueue();
        IDataResult<List<Waitlist>> GetAll();
    }
}

