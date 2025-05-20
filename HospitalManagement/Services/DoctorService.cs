using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctorService
    {   
        private readonly IDoctorRepository _repository;
        private readonly IDepartmentService _departmentService;

        public DoctorService(IDoctorRepository doctorRepository , IDepartmentService departmentService)
        {
            _repository = doctorRepository;
            _departmentService = departmentService;
        }
        public async Task<Doctor?> AddDoctorAsync(AddDoctorDto doctorDto)
        {
            try
            {
                if (doctorDto == null)
                    return null;

                //var deprtmentName =await  _departmentService.GetDepartmentByIdAsync(doctorDto.DepartmentId);
                Department department= await _departmentService.GetDepartmentByIdAsync(doctorDto.DepartmentId);
                if (department == null)
                {
                    return null;
                }

                var doctor = new Doctor
                {
                    Name = doctorDto.Name,
                    Specialization = doctorDto.Specialization,
                    AvailabilitySlot = doctorDto.AvailabilitySlot,
                    ContactDetails = doctorDto.ContactDetails,
                    DepartmentId = doctorDto.DepartmentId,
                };
                return await _repository.AddDoctorAsync(doctor);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            try
            {
                var isIdExists = _repository.GetDoctorByIdAsync(id);
                if (isIdExists == null)
                {
                    return false;
                }
                return await _repository.DeleteDoctorAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            try
            {
                var res = await _repository.GetAllDocotorsAsync();

                //var docotorList =  
                return res;
            }
            catch (Exception ex)
            {
                //logger exception
                return new List<Doctor>();
            }
        }

        public  async Task<GetDoctorDto?> GetDoctorByIdAsync(int id)
        {
            try
            {
                var res = await _repository.GetDoctorByIdAsync(id);
                    
                if(res == null)
                {
                    return null;
                }
                Department? dept = await  _departmentService.GetDepartmentByIdAsync(res.DepartmentId) ;
                var doctor = new GetDoctorDto
                {
                    Name = res.Name,
                    Specialization= res.Specialization,
                    AvailabilitySlot =res.AvailabilitySlot,
                    ContactDetails= res.ContactDetails,
                    DepartmentName = dept.Name 
                };
                return doctor;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<bool> UpdateDoctorAsync(UpdateDoctorDto doctorDto)
        {
            try
            {
                Doctor? existingDoctor = await _repository.GetDoctorByIdAsync(doctorDto.Id);

                if (existingDoctor == null)
                {
                    return false;
                }                           
                    existingDoctor.Id = doctorDto.Id;
                    existingDoctor.Name = doctorDto.Name;
                    existingDoctor.Specialization =doctorDto.Specialization;
                    existingDoctor.AvailabilitySlot = doctorDto.AvailabilitySlot ;
                    existingDoctor.ContactDetails =doctorDto.ContactDetails;             

                return await _repository.UpdateDoctorAsync(existingDoctor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateAvaialbiltySlotAsync(CreateAvailabilitySlotDto dto)
        {
            try
            {
                var doctor = _repository.GetDoctorByIdAsync(dto.DoctorId);
                if (doctor == null)
                {
                    return false;
                }

                var exisitngSlots = await _repository.GetBySlotDoctorIdAsync(dto.DoctorId);
                ///overlapping 
                ///
                
                bool isOverlapping =   exisitngSlots.Any(slot => 
                slot.DayofWeek == dto.DayofWeek && (
                    (dto.StartTime >= slot.StartTime && dto.StartTime < slot.EndTime) ||
                    (dto.EndTime > slot.StartTime && dto.EndTime <= slot.EndTime) ||
                    (dto.StartTime <= slot.StartTime && dto.EndTime >= slot.EndTime)
                )
                );

                if (isOverlapping)
                    return false;


                var slot = new AvailabilitySlot
                {
                    DoctorId = dto.DoctorId,
                    DayofWeek = dto.DayofWeek,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };
                return await _repository.CreateAvaialbiltySlotAsync(slot);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<AvailabilitySlot>> GetBySlotDoctorIdAsync(int doctorId)
        {
            try
            {
                return await _repository.GetBySlotDoctorIdAsync(doctorId);
                   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<AvailabilitySlot>();
            }
        }
    }
}
