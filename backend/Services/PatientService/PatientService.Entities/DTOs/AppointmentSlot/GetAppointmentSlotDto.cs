using PatientService.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Entities.DTOs.AppointmentSlot
{
    public class GetAppointmentSlotDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorFullName { get; set; }
        public DateOnly SlotDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public AppointmentSlotStatus Status { get; set; }
    }
}
