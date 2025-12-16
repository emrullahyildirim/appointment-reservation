using AutoMapper;
using Entities.Concrete;
using PatientService.Entities.DTOs.Appointment;
using PatientService.Entities.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Business.Mapping.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();

            CreateMap<Patient, CreatePatientDto>().ReverseMap();

        }
    }
}
