using HospitalManagement.DTO;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {

        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        [HttpGet("get/patients")]
        public async Task<IActionResult> GetPatients()
        {
            try
            {
                var res = await _patientService.GetPatientsAsync();

                if (res == null)
                {
                    return NotFound("No patients found.");
                }

                return Ok(res);
            }
            catch (Exception ex)
            {   //logexception 
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while get  patients.");
            }
        }

        [HttpGet("get/patient/{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            try
            {
               
                var res = await _patientService.GetPatientByIdAsync(id);
                if(res == null)
                {
                    return NotFound("patient not found");
                }
                return Ok(res); 

            }
            catch (Exception ex)
            {
                return BadRequest("invalid request ");
            }
        }

        [HttpPost("add/patient")]
        public async Task<IActionResult> CreatePatientAsync(PatientAddDto patientDto)
        {
            try
            {
                var res = await _patientService.AddPatientAsync(patientDto);
                if (res == false)
                {
                    return NotFound("invalid data");
                }
                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                //log 
                return StatusCode(500, "Error occured while adding Patient");
            }
        }

        [HttpGet("search/patient")]
        public async Task<IActionResult> SearchPatients(string name,  string? email = null, string? mobile =null)
        {
            var result = await _patientService.SearchPatientsAsync(name, email, mobile);
            return Ok(result);
        }
    }
}
