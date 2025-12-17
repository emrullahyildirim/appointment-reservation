using Core.Entities;
using Entities.Concrete;
using PatientService.Entities.Enums;

namespace PatientService.Entities.Concrete
{
     public class Waitlist : IEntity
     {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly PreferredDate { get; set; }
        public TimeOnly? PreferredStartTime { get; set; }
        public TimeOnly? PreferredEndTime { get; set; }
        public int QueuePosition { get; set; }
        public WaitlistStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? NotifiedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? NotifiedSlotId { get; set; }



        public virtual Patient Patient { get; set; } = null!;
        public virtual AppointmentSlot? NotifiedSlot { get; set; }

    }
}