using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Business.External.Dtos;
using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using AutoMapper;

namespace AuthService.Business.Mapping.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePatientDto, UserForRegisterDto>().ReverseMap();
        }
    }
}
