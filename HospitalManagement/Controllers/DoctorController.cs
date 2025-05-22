using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service) 
        {
            _service = service;
        }


        [Authorize (Roles ="Doctor")]
        [HttpGet("get/doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            try
            {
                var res = await _service.GetDoctorsAsync();

                if (res == null)
                {
                    return NotFound("No doctors found.");
                }

                return Ok(res);
            }
            catch (Exception ex)
            {   //logexception 
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while get  doctors.");
            }
        }


        [HttpPost("add/doctor")]
        public async Task<IActionResult> CreateDocotr(AddDoctorDto doctorDto)
        {
            try
            {
                var res = await _service.AddDoctorAsync(doctorDto);
                if (res == null)
                {
                    return NotFound("invalid data");
                }
                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                //log 
                return StatusCode(500, "Error occured while adding Doctor");
            }
        }

        [HttpPut("update/doctor")]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null)
                {
                    return BadRequest("invalid data");
                }
                var res = await _service.UpdateDoctorAsync(doctorDto);
                if (res == false)
                {
                    return BadRequest("invalid data or Id ");
                }
                return Ok("Updated succefully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("not updated ");
            }
        }

        [HttpDelete("delete/doctor/{id}")]
        public async Task<IActionResult> DeleteDepartmentById(int id)
        {
            try
            {
                var validDoct = await _service.GetDoctorByIdAsync(id);
                if (validDoct == null)
                {
                    return BadRequest("Invlaid docotr");
                }
                var res = await _service.DeleteDoctorAsync(id);
                if (res == false)
                {
                    return BadRequest("invalid id ");
                }
                return Ok("deleted succefully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("invalid id ");
            }
        }


        
        [HttpGet("get/doctor/{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(int id)
        {
            try
            {
                var res = await _service.GetDoctorByIdAsync(id);

                if (res == null)
                {
                    return NotFound("Not found");
                }
                return Ok(res);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("id is invalid");
            }
        }

        [HttpGet("avalibalitySlots/{doctorId}")]
        public async Task<IActionResult> GetAvailabilityByDoctorId(int doctorId)
        {
            try
            {
                var slots = await _service.GetBySlotDoctorIdAsync(doctorId);

                if (slots == null || !slots.Any())
                    return NotFound($"No availability slots found for doctor with ID {doctorId}.");

                return Ok(slots);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while fetching availability slots.");
            }
        }


        [HttpPost("availability/slot")]
        public async Task<IActionResult> AddAvailabilitySlot( CreateAvailabilitySlotDto dto)
        {
            try
            {
                var result = await _service.CreateAvaialbiltySlotAsync(dto);

                if (!result)
                    return BadRequest("Could not add availability slot. It might be overlapping or invalid.");

                return Ok("Availability slot added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while adding the availability slot.");
            }
        }
    }
}
