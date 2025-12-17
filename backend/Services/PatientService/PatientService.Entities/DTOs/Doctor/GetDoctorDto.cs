using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Entities.DTOs.Doctor
{
    public class GetDoctorDto
    {
        public int Id { get; set; }
        public string DoctorTitleName { get; set; }
        public string DoctorFullName { get; set; }

    }
}
