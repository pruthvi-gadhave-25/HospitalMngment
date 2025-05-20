using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;

namespace HospitalManagement.Services.Interface
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctorsAsync();
        Task<bool> UpdateDoctorAsync(UpdateDoctorDto doctor);
        Task<bool> DeleteDoctorAsync(int id);
        Task<GetDoctorDto?> GetDoctorByIdAsync(int id);
        Task<Doctor?> AddDoctorAsync(AddDoctorDto doctorDto);
        Task<bool> CreateAvaialbiltySlotAsync(CreateAvailabilitySlotDto dto);
        Task<List<AvailabilitySlot>> GetBySlotDoctorIdAsync(int doctorId);
    }
}
