using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;

namespace HospitalManagement.Services.Interface
{
    public interface IDoctorService
    {
        Task<Result<List<GetDoctorDto>>> GetDoctorsAsync();
        Task<Result<bool>> UpdateDoctorAsync(UpdateDoctorDto doctor);
        Task<Result<bool>> DeleteDoctorAsync(int id);
        Task<Result<GetDoctorDto>> GetDoctorByIdAsync(int id);
        Task<Result<Doctor>> AddDoctorAsync(AddDoctorDto doctorDto);
        Task<Result<bool>> CreateAvaialbiltySlotAsync(CreateAvailabilitySlotDto dto);
        Task<Result<List<GetAvailabilitySlotDto>>> GetBySlotDoctorIdAsync(int doctorId);
    }
}
