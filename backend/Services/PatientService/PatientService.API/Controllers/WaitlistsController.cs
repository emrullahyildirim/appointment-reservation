using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Business.Abstract;
using PatientService.DataAccess.Abstract;
using PatientService.Entities.Concrete;
using PatientService.Entities.DTOs.Waitlist;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitlistsController : ControllerBase
    {
        private readonly IWaitlistService _waitlistService;
        private readonly IMapper _mapper;
        public WaitlistsController(IWaitlistService waitlistService, IMapper mapper)
        {
            _waitlistService = waitlistService;
            _mapper = mapper;
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var result = _waitlistService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }



        [HttpPost]
        public IActionResult Add([FromBody] AddToWaitlistDto addToWaitlistDto)
        {
            var waitlistEntity = _mapper.Map<Waitlist>(addToWaitlistDto);
            var result = _waitlistService.Add(waitlistEntity);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }



        [HttpDelete]
        public IActionResult CancelWaitlist(int waitlistId)
        {
            var result = _waitlistService.CancelWaitlist(waitlistId);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }



        [HttpGet("byPatientId")]
        public  IActionResult GetWaitlistByPatientId(int patientId)
        {
            var result = _waitlistService.GetWaitlistByPatientId(patientId);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
