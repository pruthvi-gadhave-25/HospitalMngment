using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Migrations;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;

namespace HospitalManagement.Repository.Interface
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task<List<Doctor>> GetAllDocotorsAsync();
        Task<Doctor?> AddDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<bool> DeleteDoctorAsync(int id);

        Task<bool> CreateAvaialbiltySlotAsync(AvailabilitySlot slot);
        Task<List<AvailabilitySlot>> GetBySlotDoctorIdAsync(int doctorId);
    }
}
