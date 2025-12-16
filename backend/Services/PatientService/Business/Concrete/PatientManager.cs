using Business.Abstract;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class PatientManager : IPatientService
    {
        IPatientDal _patientDal;

        public PatientManager(IPatientDal patientDal)
        {
            _patientDal = patientDal;
        }

        public IResult Add(Patient patient)
        {
            _patientDal.Add(patient);
            return new SuccessResult(" eklendi");
        }

        public IResult Delete(Patient patient)
        {
            _patientDal.Remove(patient);
            return new SuccessResult();
        }

        public IDataResult<List<Patient>> GetAll()
        {
            return new SuccessDataResult<List<Patient>>(_patientDal.GetAll(),"Message");
        }

        public IDataResult<Patient> GetById(int id)
        {
            return new SuccessDataResult<Patient>(_patientDal.Get(p => p.Id == id));
        }

        public IResult Update(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}

