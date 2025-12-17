using AutoMapper;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Entities.DTOs.AppointmentSlot;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentSlotsController : ControllerBase
    {

        private readonly IAppointmentSlotService _appointmentSlotService;
        private readonly IMapper _mapper;
        public AppointmentSlotsController(IAppointmentSlotService appointmentSlotService, IMapper mapper)
        {
            _appointmentSlotService = appointmentSlotService;
            _mapper = mapper;
        }


        [HttpGet("all")]
        public IActionResult Get()
        {
            var result = _appointmentSlotService.GetAll();
            var mapped = _mapper.Map<List<GetAppointmentSlotDto>>(result.Data);
            if (result.IsSuccess)
            {
                return Ok(mapped);
            }
            return BadRequest(result);
        }


        [HttpGet("healtcheck")]
        public IActionResult HealthCheck()
        {
            return Ok("AppointmentSlot Service is running.");
        }



        [HttpGet("byDoctorId")]
        public IActionResult GetByDoctorId(int id)
        {
            var result = _appointmentSlotService.GetAllByDoctorId(id);
            var mapped = _mapper.Map<List<GetAppointmentSlotDto>>(result.Data);
            if (result.IsSuccess)
            {
                return Ok(mapped);
            }
            return BadRequest(result);
        }





    }
}
