using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Business.Abstract;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var result = _doctorService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("appointments")]
        public IActionResult GetAppointments(int doctorId)
        {
            var result = _doctorService.GetAppointmentsByDoctorId(doctorId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
