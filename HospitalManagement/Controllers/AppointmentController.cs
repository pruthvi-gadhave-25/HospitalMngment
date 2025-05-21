using HospitalManagement.DTO;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {   
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookAppointmet(BookAppointmentDto appointmentDto)
        {
            try
            {
                var res = await _appointmentService.BookAppointment(appointmentDto);
                if(res == false)
                {
                    return BadRequest("Appointmentnot not booked ");
                }
                return Ok("appointment booked succefully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
               var res =  await _appointmentService.CancelApppointment(appointmentId);
                if(res == false)
                {
                    return BadRequest("failed to cancel appointment");
                }
                return Ok("cancelled succcfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);  
            }
        }

        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            try
            {
                 var res =await  _appointmentService.RescheduleApppointment(rescheduleAppointmentDto);
                if(res == false)
                {
                    return BadRequest("failed to rescheduled appointment");
                }
                return Ok("rescheduled succefully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentAsync();
                if(appointments == null)
                {
                    return NotFound();
                }
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                //log edexception 

                return BadRequest("error occured");
            }
        }

        [HttpGet("get/bydoctor")]
        public async Task<IActionResult> GetAppointByDoctorId(int doctorId )
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentBtDoctorAsync(doctorId);
                if (appointment == null)
                {
                    return NotFound("appointment Not Found");
                }
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                //log edexception 

                return BadRequest("error occured");
            }
        }

        [HttpGet("get/date")]
        public async Task<IActionResult> GetAppointmentByDate(DateTime dateTime)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByDateAsync(dateTime);
                if (appointment == null)
                {
                    return NotFound("appointment not found");
                }
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                //log edexception 

                return BadRequest("error occured");
            }
        }
    }
}
