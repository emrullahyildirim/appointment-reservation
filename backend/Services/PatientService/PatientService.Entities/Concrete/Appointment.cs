using Core.Entities;
using PatientService.Entities.Enums;

namespace Entities.Concrete
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SlotId { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } 


        public virtual Patient Patient { get; set; } = null!;
        public virtual AppointmentSlot AppointmentSlot { get; set; } = null!;

    }
}