using Core.Utilities.Result;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPatientService
    {
        IResult Add(Patient patient);
        IResult Update(Patient patient);
        IResult Delete(Patient patient);
        IDataResult<Patient> GetById(int id);
        IDataResult<List<Patient>> GetAll();
    }
}

