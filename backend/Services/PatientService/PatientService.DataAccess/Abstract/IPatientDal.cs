using Core.DataAcces;
using Entities.Concrete;

namespace DataAccess.Abstract
{
     public interface IPatientDal : IEntityRepository<Patient>
    {
    }
}