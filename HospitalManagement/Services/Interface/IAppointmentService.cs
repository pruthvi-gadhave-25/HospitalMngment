using HospitalManagement.DTO;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IAppointmentService
    {   
        Task<bool> BookAppointment(BookAppointmentDto appointmentDto);
        Task<bool> CancelApppointment(int appointmentId);
        Task<bool> RescheduleApppointment(RescheduleAppointmentDto rescheduleAppointmentDto);
        Task<List<Appointment>> GetAppointmentAsync();

        Task<List<Appointment>> GetAppointmentByDateAsync(DateTime date);
        Task<List<Appointment>> GetAppointmentBtDoctorAsync(int doctorId);
    }
}
