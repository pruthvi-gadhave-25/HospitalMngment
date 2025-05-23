using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Helpers;
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


        //[Authorize (Roles ="Doctor")]
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
                var res = await _service.AddDoctorAsync(doctorDto);
            if (res == null)
                return ApiResponseHelper.CreatFailure(res.Message, 400);

            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);                                    
        }

        [HttpPut("update/doctor")]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctorDto doctorDto)
        {           
                if (doctorDto == null)
                {
                    return ApiResponseHelper.CreatFailure("inalvid doctor ", 400);
                }
                var res = await _service.UpdateDoctorAsync(doctorDto);
                if (!res.IsSuccess )
                {
                    return ApiResponseHelper.CreatFailure("invalid data or Id ", 400);
                }
                return ApiResponseHelper.CreateSuccess(res.Data ,res.Message);
                   
        }

        [HttpDelete("delete/doctor/{id}")]
        public async Task<IActionResult> DeleteDepartmentById(int id)
        {
                var validDoct = await _service.GetDoctorByIdAsync(id);
                if (validDoct == null)
                {
                    return ApiResponseHelper.CreatFailure("invlaid doctor id ", 400);
                }
                var res = await _service.DeleteDoctorAsync(id);
                if (res.IsSuccess == false)
                {
                    return ApiResponseHelper.CreatFailure("invalid id", 400);
                }
                return ApiResponseHelper.CreateSuccess(res.Data, res.Message);

        }


        
        [HttpGet("get/doctor/{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(int id)
        {
                var res = await _service.GetDoctorByIdAsync(id);

                if (res.Data == null)
                {
                    return ApiResponseHelper.CreatFailure(res.Message, 400);
                }
                return  ApiResponseHelper.CreateSuccess(res.Data, res.Message);  
            
        }

        [HttpGet("avalibalitySlots/{doctorId}")]
        public async Task<IActionResult> GetAvailabilityByDoctorId(int doctorId)
        {
                var slots = await _service.GetBySlotDoctorIdAsync(doctorId);
                
            if(slots.Data.Count == 0)
            {
                return ApiResponseHelper.CreateSuccess(slots.Data, slots.Message); 
            }
            if (slots == null || !slots.Data.Any())
            return ApiResponseHelper.CreatFailure(slots.Message, 400);


            return ApiResponseHelper.CreateSuccess(slots.Data, slots.Message); ;
            
        }


        [HttpPost("availability/slot/add")]
        public async Task<IActionResult> AddAvailabilitySlot( CreateAvailabilitySlotDto dto)
        {
            try
            {
                var result = await _service.CreateAvaialbiltySlotAsync(dto);

                if (!result.IsSuccess)
                    return ApiResponseHelper.CreatFailure(result.Message, 400);

                return  ApiResponseHelper.CreateSuccess(result.Data, result.Message); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while adding the availability slot.");
            }
        }
    }
}
