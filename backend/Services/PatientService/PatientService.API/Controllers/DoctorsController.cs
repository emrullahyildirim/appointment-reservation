using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Business.Abstract;
using PatientService.Entities.DTOs.Appointment;
using PatientService.Entities.DTOs.Doctor;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        public DoctorsController(IDoctorService doctorService, IMapper mapper)
        {
            _doctorService = doctorService;
            _mapper = mapper;
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var result = _doctorService.GetAll();
            var mapped = _mapper.Map<List<GetDoctorDto>>(result.Data);
            if (result.IsSuccess)
            {
                return Ok(mapped);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("appointments")]
        public IActionResult GetAppointments(int doctorId)
        {
            var result = _doctorService.GetAppointmentsByDoctorId(doctorId);
            var mapped = _mapper.Map<List<GetAppointmentDto>>(result.Data);
            if (result.IsSuccess)
            {
                return Ok(mapped);
            }
            return BadRequest(result.Message);
        }
    }
}
