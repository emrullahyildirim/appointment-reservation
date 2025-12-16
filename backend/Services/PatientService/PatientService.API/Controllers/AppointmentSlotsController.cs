using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentSlotsController : ControllerBase
    {

        private readonly IAppointmentSlotService _appointmentSlotService;

        public AppointmentSlotsController(IAppointmentSlotService appointmentSlotService)
        {
            _appointmentSlotService = appointmentSlotService;
        }


        [HttpGet("all")]
        public IActionResult Get()
        {
            var result = _appointmentSlotService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
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
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }





    }
}
