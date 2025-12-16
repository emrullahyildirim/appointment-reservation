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
