using HospitalManagement.DTO;
using HospitalManagement.Helpers;
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
        Task<List<Appointment>> GetDailyAppointmetsByDoctor(int doctorId ,DateTime date , int pageIndex , int pageSize);
        Task<List<Appointment>> GetDailyAppointmetsByDepartment(int departmentId ,DateTime date , int pageIndex , int pageSize);
        Task<bool> UpdateAppointentAsync(int appointmentId);

        Task<List<Appointment>> GetPatientVisitCountAsync(int patientId);

    }

}
