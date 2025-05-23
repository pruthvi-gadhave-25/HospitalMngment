using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IAppointmentService
    {   
        Task <Result<bool>> BookAppointment(BookAppointmentDto appointmentDto);
        Task<Result<bool>> CancelApppointment(int appointmentId);
        Task<Result<bool>> RescheduleApppointment(RescheduleAppointmentDto rescheduleAppointmentDto);
        Task<Result<List<GetAppointmentsDto>>> GetAppointmentAsync();

        Task<Result<List<GetAppointmentsDto>>> GetAppointmentByDateAsync(DateTime date);
        Task<Result<List<GetAppointmentsDto>>> GetAppointmentBtDoctorAsync(int doctorId);
    }
}
