using AutoMapper;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Entities.DTOs.Appointment;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        public AppointmentsController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }


        [HttpGet("all")]
        public IActionResult Get()
        {
            var result = _appointmentService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("healtcheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Appointment Service is running.");
        }



        [HttpGet("history")]
        public IActionResult GetHistories(int patientId)
        {
            var result = _appointmentService.GetPacientHistories(patientId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(CreateAppointmentDto appointmentCreateDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentCreateDto);
            //if (!int.TryParse(Request.Headers["UserId"], out int patientId))
            //{
            //    return BadRequest("Invalid UserId header");
            //}
            //appointment.PatientId = patientId;
            var result = _appointmentService.Add(appointment);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
