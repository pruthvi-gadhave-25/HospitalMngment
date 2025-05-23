using HospitalManagement.DTO;
using HospitalManagement.Helpers;
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
            var res = await _appointmentService.BookAppointment(appointmentDto);
            if(res.IsSuccess == false)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
                    
            return ApiResponseHelper.CreateSuccess(res.Data,res.Message);
        }
        

        [HttpDelete("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var res =  await _appointmentService.CancelApppointment(appointmentId);
            if(res.IsSuccess == false)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data ,res.Message);

        }

        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentDto rescheduleAppointmentDto)
        {   
            var res =await  _appointmentService.RescheduleApppointment(rescheduleAppointmentDto);
            if(res.IsSuccess == false)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);          
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentAsync();
            if(appointments == null)
            {
                return ApiResponseHelper.CreatFailure(appointments.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(appointments.Data, appointments.Message);            
        }

        [HttpGet("get/bydoctor")]
        public async Task<IActionResult> GetAppointByDoctorId(int doctorId )
        {
            var appointment = await _appointmentService.GetAppointmentBtDoctorAsync(doctorId);
            if (appointment == null)
            {
            return ApiResponseHelper.CreatFailure(appointment.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(appointment.Data, appointment.Message);
        }

        [HttpGet("get/date")]
        public async Task<IActionResult> GetAppointmentByDate(DateTime dateTime)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByDateAsync(dateTime);
                if (appointment == null)
                {
                    return ApiResponseHelper.CreatFailure(appointment.Message, 400);
                }
                return ApiResponseHelper.CreateSuccess(appointment.Data ,appointment.Message, 400);
            }
            catch (Exception ex)
            {
                //log edexception 

                return BadRequest("error occured");
            }
        }
    }
}
