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
        Task<Result<bool>> UpdateAppointmentStatus(int appointmentId);
        Task<Result<List<GetAppointmentsDto>>> GetDailyAppointmentByDocotorAsync(int doctorId , DateTime date , int pageIndex ,int pageSize);
        Task<Result<List<GetAppointmentsDto>>> GetDailyAppointmentByDepartmentAsync(int departmentId , DateTime date , int pageIndex ,int pageSize);
        Task<Result<VisitsAddPatientDto>> GetVisitCountPatientAysnc(int patientId);
    }
}
