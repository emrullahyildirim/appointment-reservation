using Core.DataAcces.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using PatientService.DataAccess.Concrete.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPatientDal : EfEntityRepositoryBase<Patient, PatientAppointmentContext>, IPatientDal
    {

    }
}