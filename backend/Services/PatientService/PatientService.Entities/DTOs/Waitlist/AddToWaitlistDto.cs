using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Entities.DTOs.Waitlist
{
    public class AddToWaitlistDto : IDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly PreferredDate { get; set; }
        public TimeOnly? PreferredStartTime { get; set; }
        public TimeOnly? PreferredEndTime { get; set; }
    }
}
