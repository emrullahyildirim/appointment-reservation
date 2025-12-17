using AutoMapper;
using Entities.Concrete;
using PatientService.Entities.Concrete;
using PatientService.Entities.DTOs.Appointment;
using PatientService.Entities.DTOs.AppointmentSlot;
using PatientService.Entities.DTOs.Doctor;
using PatientService.Entities.DTOs.Patient;
using PatientService.Entities.DTOs.Waitlist;
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
            CreateMap<Appointment, GetAppointmentDto>()
                    .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Doctor.FirstName + " " + src.Doctor.LastName));
            

            CreateMap<GetAppointmentSlotDto, AppointmentSlot>().ReverseMap()
                .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Doctor.FirstName + " " + src.Doctor.LastName)); 


            CreateMap<Patient, CreatePatientDto>().ReverseMap();
            CreateMap<Waitlist, AddToWaitlistDto>().ReverseMap();


            CreateMap<Doctor, GetDoctorDto>()
                .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.DoctorTitleName, opt => opt.MapFrom(src => src.DoctorTitle.TitleName));


        }
    }
}
