using Core.Entities;

namespace PatientService.Entities.Concrete
{
    public class DoctorTitle : IEntity
    {
        public int Id { get; set; }
        public string TitleName { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}