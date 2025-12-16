using Core.Entities;
using PatientService.Entities.Enums;

namespace Entities.Concrete
{
    public class AppointmentSlot : IEntity
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateOnly SlotDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public AppointmentSlotStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Appointment? Appointment { get; set; }

    }
}