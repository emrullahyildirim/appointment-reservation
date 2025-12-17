using Core.Utilities.Result;
using PatientService.Entities.Concrete;

namespace PatientService.Business.Abstract
{
    public interface IDoctorTitleService
    {
        IResult Add(DoctorTitle doctortitle);
        IResult Update(DoctorTitle doctortitle);
        IResult Delete(DoctorTitle doctortitle);
        IDataResult<DoctorTitle> GetById(int id);
        IDataResult<List<DoctorTitle>> GetAll();
    }
}

