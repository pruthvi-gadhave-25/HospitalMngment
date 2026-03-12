using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IPatientService
    {
        Task<bool> AddPatientAsync(PatientAddDto patientDto);
        Task<Result<PagedResult<GetPatientDto>>> GetPatientsAsync(int pageIndex = 1, int pageSize = 10);
        Task<Result<GetPatientDto>> GetPatientByIdAsync(int id);
        Task<Result<List<Patient>>> SearchPatientsAsync(string? name, string? email, string? mobile);
        Task<Result<bool>> UpdatePatientAsync(UpdatePatientDto updateDto);
        Task<Result<bool>> DeletePatientAsync(int id);
    }
}
