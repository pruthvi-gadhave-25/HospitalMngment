using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IPatientService
    {
        Task<bool> AddPatientAsync(PatientAddDto patientDto);
        Task<List<GetPatientDto>> GetPatientsAsync();
        Task<Result<GetPatientDto>> GetPatientByIdAsync(int id);
        Task<List<Patient>> SearchPatientsAsync(string? name, string? email, string? mobile);
    }
}
