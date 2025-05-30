using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    //[Authorize(Roles = "Admin ,Doctor ,Receptionist , Patients")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpPost("book/appointment")]
        public async Task<IActionResult> BookAppointmet(BookAppointmentDto appointmentDto)
        {
            var res = await _appointmentService.BookAppointment(appointmentDto);
            if (res.IsSuccess == false)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }

            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);
        }


        [HttpDelete("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var res = await _appointmentService.CancelApppointment(appointmentId);
            if (res.IsSuccess == false)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);

        }

        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            var res = await _appointmentService.RescheduleApppointment(rescheduleAppointmentDto);
            if (res.IsSuccess == false)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);
        }



        [HttpGet("get/appointmets")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentAsync();
            if (appointments == null)
            {
                return ApiResponseHelper.CreatFailure(appointments.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(appointments.Data, appointments.Message);
        }

        [HttpGet("get/bydoctor")]
        public async Task<IActionResult> GetAppointByDoctorId(int doctorId)
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
                return ApiResponseHelper.CreateSuccess(appointment.Data, appointment.Message, 400);
            }
            catch (Exception ex)
            {
                //log edexception 

                return BadRequest("error occured");
            }
        }


        [HttpGet("get/daily/appointments/by/doctor/{doctorId}")]
        public async Task<IActionResult> GetDailyAppointmentsByDocotor(int doctorId, DateTime dateTime, int pageIndex, int pageSize)
        {
            var res = await _appointmentService.GetDailyAppointmentByDocotorAsync(doctorId, dateTime, pageIndex, pageSize);

            if (res == null)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);
        }

        [HttpGet("get/daily/appointments/by/department/{departmentId}")]
        public async Task<IActionResult> GetDailyAppointmentsByDepartmentId(int departmentId, DateTime dateTime, int pageIndex, int pageSize)
        {
            var res = await _appointmentService.GetDailyAppointmentByDocotorAsync(departmentId, dateTime, pageIndex, pageSize);

            if (res == null)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);
        }


        [HttpPut("appointments/{appointmentId}/complete")]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {

            var res = await _appointmentService.UpdateAppointmentStatus(appointmentId);
            if (!res.IsSuccess)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);

        }

        [HttpGet("appointments/visits/{patientId}")]
        public async Task<IActionResult> GetPatitntVisits(int patientId)
        {
            var res = await _appointmentService.GetVisitCountPatientAysnc(patientId);

           if(!res.IsSuccess)
           {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
           }
                
            return ApiResponseHelper.CreateSuccess(res.Data, "visits of patient");
        }
    }
}
