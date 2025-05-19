using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IPatientService
    {
        Task<bool> AddPatientAsync(Patient patient);
        Task<List<Patient>> GetPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
    }
}
