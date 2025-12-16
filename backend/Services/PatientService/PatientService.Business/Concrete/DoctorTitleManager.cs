using PatientService.Business.Abstract;
using Core.Utilities.Result;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;

namespace PatientService.Business.Concrete
{
    public class DoctorTitleManager : IDoctorTitleService
    {
        IDoctorTitleDal _doctortitleDal;

        public DoctorTitleManager(IDoctorTitleDal doctortitleDal)
        {
            _doctortitleDal = doctortitleDal;
        }

        public IResult Add(DoctorTitle doctortitle)
        {
            _doctortitleDal.Add(doctortitle);
            return new SuccessResult(" eklendi");
        }

        public IResult Delete(DoctorTitle doctortitle)
        {
            _doctortitleDal.Remove(doctortitle);
            return new SuccessResult();
        }

        public IDataResult<List<DoctorTitle>> GetAll()
        {
            return new SuccessDataResult<List<DoctorTitle>>(_doctortitleDal.GetAll(),"Message");
        }

        public IDataResult<DoctorTitle> GetById(int id)
        {
            return new SuccessDataResult<DoctorTitle>(_doctortitleDal.Get(p => p.Id == id));
        }

        public IResult Update(DoctorTitle doctortitle)
        {
            throw new NotImplementedException();
        }
    }
}

