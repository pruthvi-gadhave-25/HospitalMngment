using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IPatientRepository
    {
        Task<bool> AddPatientAsync(Patient patient);
        Task<List<Patient>> GetPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);

        Task<List<Patient>> SearchPatientAsync(string name, string mobileNo, string email);
    }
}
