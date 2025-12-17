using Core.Entities;
using Entities.Concrete;

namespace PatientService.Entities.Concrete
{
    public class Doctor : IEntity
    {
        public int Id { get; set; }
        public int DoctorTitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual DoctorTitle DoctorTitle { get; set; } = null!;
        public virtual ICollection<AppointmentSlot> AppointmentSlots { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }


    }
}