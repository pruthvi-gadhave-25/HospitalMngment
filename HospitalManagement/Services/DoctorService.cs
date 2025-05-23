using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Helpers;
using HospitalManagement.Migrations;
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
        public async Task<Result<Doctor>> AddDoctorAsync(AddDoctorDto doctorDto)
        {
            
            if (doctorDto == null)
                return Result<Doctor>.ErrorResult("Doctor is required" );

            var resData = await _departmentService.GetDepartmentByIdAsync(doctorDto.DepartmentId);
            
            if (resData.IsSuccess == null)
                return Result<Doctor>.ErrorResult("invalid department id");

            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                Specialization = doctorDto.Specialization,
                AvailabilitySlot = doctorDto.AvailabilitySlot,
                ContactDetails = doctorDto.ContactDetails,
                DepartmentId = doctorDto.DepartmentId,
            };

            var res =  await _repository.AddDoctorAsync(doctor);

            return Result<Doctor>.SuccessResult(res, "Doctor Added Succefully");
                                                          
        }

        public async Task<Result<bool>> DeleteDoctorAsync(int id)
        {          
            var isIdExists = _repository.GetDoctorByIdAsync(id);
            if (isIdExists == null)
            {
            return Result<bool>.ErrorResult("invliad doctor Id ");
            }
            var res =  await _repository.DeleteDoctorAsync(id);

            return Result<bool>.SuccessResult(res, "Deleted succefully");
           
        }

        public async Task<Result<List<GetDoctorDto>>> GetDoctorsAsync()
        {
              var doctors  = await _repository.GetAllDocotorsAsync();
                if (doctors == null)
                {
                    return Result<List<GetDoctorDto>>.ErrorResult("doctors not found");
                }
                var doctorDtos = doctors.Select(d => new GetDoctorDto
                {
                    Name = d.Name,
                    Specialization = d.Specialization,
                    ContactDetails = d.ContactDetails,
                    DepartmentName = d.Department?.Name ?? "N/A",
                    AvailabilitySlots = d.AvailabilitySlots?.Select(slot => new GetAvailabilitySlotDto
                    {
                        DayofWeek = slot.DayofWeek,
                        EndTime = slot.EndTime,
                        StartTime = slot.StartTime,
                    }).ToList() ?? new()
                }).ToList();
                return  Result<List<GetDoctorDto>>.SuccessResult(doctorDtos ,"doctors fecthed successfully"); ;
            
        }

        public  async Task<Result<GetDoctorDto>> GetDoctorByIdAsync(int id)
        {
            var res = await _repository.GetDoctorByIdAsync(id);
                    
            if(res == null)
            {
                return Result<GetDoctorDto>.ErrorResult("invliad id ");
            }
            var resData = await _departmentService.GetDepartmentByIdAsync(res.DepartmentId) ;
            var doctor = new GetDoctorDto
            {
                Name = res.Name,
                Specialization= res.Specialization,
                ContactDetails= res.ContactDetails,
                DepartmentName = resData.Data.Name ,
                    
                AvailabilitySlots = res.AvailabilitySlots?.Select(slot => new GetAvailabilitySlotDto
                {
                    DayofWeek = slot.DayofWeek,
                    EndTime = slot.EndTime,
                    StartTime = slot.StartTime,
                }).ToList() ?? new()
                    
            };
            return Result<GetDoctorDto>.SuccessResult(doctor, "Fetched Succfully");
            
        }

        public async  Task<Result<bool>> UpdateDoctorAsync(UpdateDoctorDto doctorDto)
        {
           
                Doctor? existingDoctor = await _repository.GetDoctorByIdAsync(doctorDto.Id);

                if (existingDoctor == null)
                {
                    return Result<bool>.ErrorResult("doctor is invalid");
                }                           
                    existingDoctor.Id = doctorDto.Id;
                    existingDoctor.Name = doctorDto.Name;
                    existingDoctor.Specialization =doctorDto.Specialization;
                    existingDoctor.AvailabilitySlot = doctorDto.AvailabilitySlot ;
                    existingDoctor.ContactDetails =doctorDto.ContactDetails;             

               var res =  await _repository.UpdateDoctorAsync(existingDoctor);

            return Result<bool>.SuccessResult(res, "Updated Succesfully");



        }

        public async Task<Result<bool>> CreateAvaialbiltySlotAsync(CreateAvailabilitySlotDto dto)
        {
                var doctor = _repository.GetDoctorByIdAsync(dto.DoctorId);
                if (doctor == null)
                {
                    return Result<bool>.ErrorResult("invalid doctor id ");
                }

                var exisitngSlots = await GetBySlotDoctorIdAsync(dto.DoctorId);
                
                
                bool isOverlapping =   exisitngSlots.Data.Any(slot => 
                slot.DayofWeek == dto.DayofWeek && (
                    (dto.StartTime >= slot.StartTime && dto.StartTime < slot.EndTime) ||
                    (dto.EndTime > slot.StartTime && dto.EndTime <= slot.EndTime) ||
                    (dto.StartTime <= slot.StartTime && dto.EndTime >= slot.EndTime)
                )
                );

                if (isOverlapping)
                    return Result<bool>.ErrorResult("Could not add availability slot. It might be overlapping or invalid.");


                var slot = new AvailabilitySlot
                {
                    DoctorId = dto.DoctorId,
                    DayofWeek = dto.DayofWeek,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };
               var res =  await _repository.CreateAvaialbiltySlotAsync(slot);
                return Result<bool>.SuccessResult(res, "slot added succfully");
            
        }

        public async Task<Result<List<GetAvailabilitySlotDto>>> GetBySlotDoctorIdAsync(int doctorId)
        {
            var isValidDoctor =await _repository.GetDoctorByIdAsync(doctorId);
            if(isValidDoctor == null)
            {
                return Result<List<GetAvailabilitySlotDto>>.ErrorResult("invlid doctor id ");
            }
                var res =  await _repository.GetBySlotDoctorIdAsync(doctorId);

            var avaialbalitySlotsDto = res.Select(a => new GetAvailabilitySlotDto
            {
                DayofWeek = a.DayofWeek,
                StartTime = a.StartTime,
                EndTime = a.EndTime                
            }).ToList() ?? new();

            if(res.Count == 0)
            {
                return Result<List<GetAvailabilitySlotDto>>.SuccessResult(avaialbalitySlotsDto, $"no avalibility slot found for {doctorId}");
            }
            return Result<List<GetAvailabilitySlotDto>>.SuccessResult(avaialbalitySlotsDto, "avalibilty slot fetched succfully");
           
        }
    }
}
