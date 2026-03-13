using HospitalManagement.Interface;
using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetPatientByIdAsync(int id);
        Task<List<Patient>> GetPatientsAsync();
        Task<List<Patient>> SearchPatientAsync(string? name);
    }
}
