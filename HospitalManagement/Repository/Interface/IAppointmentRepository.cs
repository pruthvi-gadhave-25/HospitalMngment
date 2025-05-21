using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetAppointmentAsync(int id);
        Task<bool> BookAppointment(Appointment appointment);
        Task<bool> CancelAppointment(int appointmentId);
        Task<bool> RescheduleAppointment(int appointmentId , DateTime newDate ,TimeOnly newTime);
        Task<List<Appointment>> GetAllAppointmentAsync();
        Task<List<Appointment>> GetAppointmentByDateAsync(DateTime date);
        Task<List<Appointment>> GetAppointmentBtDoctorAsync(int doctorId);
    }

}
