using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Entities.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int SlotId { get; set; }
    }
}
