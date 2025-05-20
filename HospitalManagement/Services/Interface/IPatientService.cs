using HospitalManagement.DTO;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IPatientService
    {
        Task<bool> AddPatientAsync(PatientAddDto patientDto);
        Task<List<Patient>> GetPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task<List<Patient>> SearchPatientsAsync(string name, string? email, string? mobile);
    }
}
