using AutoMapper;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Business.Abstract;
using PatientService.Entities.DTOs.Patient;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var result = _patientService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpPost]
        public IActionResult AddPatient(CreatePatientDto patientCreateDto)
        {
            var patient = _mapper.Map<Patient>(patientCreateDto);
            var result = _patientService.Add(patient);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
