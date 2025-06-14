﻿using HospitalManagement.DTO;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Helpers;
using HospitalManagement.Migrations;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Numerics;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctorService
    {   
        private readonly DoctorRepository _repository;
        private readonly DepartmentRepository _departmentRepso; 
        private readonly ILogger<DoctorService> _logger;

        private readonly DoctorRepository doctorRepository; 

        public DoctorService(DoctorRepository doctorRepository , 
            ILogger<DoctorService> logger ,
            DepartmentRepository departmentRepository
            )

        {
            _repository = doctorRepository;
            _logger = logger;
            _departmentRepso = departmentRepository;
        }
        public async Task<Result<Doctor>> AddDoctorAsync(AddDoctorDto doctorDto)
        {

            if (doctorDto == null)
            {
                _logger.LogError("Doctor is required");
                return Result<Doctor>.ErrorResult("Doctor is required");
            }
            var resData = await _departmentRepso.GetById(doctorDto.DepartmentId);

            if (resData == null)
            {
                _logger.LogError("Invaid Dept id ");
                return Result<Doctor>.ErrorResult("invalid department id");
            }
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                Specialization = doctorDto.Specialization,
                AvailabilitySlot = doctorDto.AvailabilitySlot,
                ContactDetails = doctorDto.ContactDetails,
                DepartmentId = doctorDto.DepartmentId,
            };

            //var res =  await _repository.AddDoctorAsync(doctor); //resturning doctor?  
            bool isSaved =  await _repository.Add(doctor);// 
            if (!isSaved)
                return Result<Doctor>.ErrorResult("Failed to Add");


            _logger.LogInformation("Doctor added succefully");
            return Result<Doctor>.SuccessResult(doctor, "Doctor Added Succefully");
                                                          
        }

        public async Task<Result<bool>> DeleteDoctorAsync(int id)
        {          
            var isIdExists = _repository.GetDoctorByIdAsync(id);
            if (isIdExists == null)
            {
                _logger.LogError("Invalid dctor Id ");
            return Result<bool>.ErrorResult("invliad doctor Id ");
            }
            var res =  await _repository.DeleteDoctorAsync(id);

            _logger.LogInformation("deleted Succefully ");
            return Result<bool>.SuccessResult(res, "Deleted succefully");
           
        }

        public async Task<Result<List<GetDoctorDto>>> GetDoctorsAsync()
        {
              var doctors  = await _repository.GetAllDocotorsAsync();
                if (doctors == null)
                {
                _logger.LogError("Doctors not found");
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

            _logger.LogInformation("doctor fetched succefully");
                return  Result<List<GetDoctorDto>>.SuccessResult(doctorDtos ,"doctors fecthed successfully"); ;
            
        }

        public  async Task<Result<GetDoctorDto>> GetDoctorByIdAsync(int id)
        {
            var res = await _repository.GetDoctorByIdAsync(id);
                    
            if(res == null)
            {
                _logger.LogError("Invalid  Id ");
                return Result<GetDoctorDto>.ErrorResult("invliad id ");
            }
            var resData = await _departmentRepso.GetById(res.DepartmentId) ;
            var doctor = new GetDoctorDto
            {
                Name = res.Name,
                Specialization= res.Specialization,
                ContactDetails= res.ContactDetails,
                DepartmentName = resData.Name ,
                    
                AvailabilitySlots = res.AvailabilitySlots?.Select(slot => new GetAvailabilitySlotDto
                {
                    DayofWeek = slot.DayofWeek,
                    EndTime = slot.EndTime,
                    StartTime = slot.StartTime,
                }).ToList() ?? new()
                    
            };
            _logger.LogInformation("fecthed succefully ");
            return Result<GetDoctorDto>.SuccessResult(doctor, "Fetched Succfully");
            
        }

        public async  Task<Result<bool>> UpdateDoctorAsync(UpdateDoctorDto doctorDto)
        {
           
                Doctor? existingDoctor = await _repository.GetDoctorByIdAsync(doctorDto.Id);

                if (existingDoctor == null)
                {
                _logger.LogError("Invalid dctor Id ");
                return Result<bool>.ErrorResult("doctor is invalid");
                }                           
                    existingDoctor.Id = doctorDto.Id;
                    existingDoctor.Name = doctorDto.Name;
                    existingDoctor.Specialization =doctorDto.Specialization;
                    existingDoctor.AvailabilitySlot = doctorDto.AvailabilitySlot ;
                    existingDoctor.ContactDetails =doctorDto.ContactDetails;             

               var res =  await _repository.UpdateDoctorAsync(existingDoctor);

            _logger.LogInformation("Updated Doctor ");
            return Result<bool>.SuccessResult(res, "Updated Succesfully");



        }

        public async Task<Result<bool>> CreateAvaialbiltySlotAsync(CreateAvailabilitySlotDto dto)
        {
                var doctor = await _repository.GetDoctorByIdAsync(dto.DoctorId);
                if (doctor == null)
                {
                _logger.LogError("Invalid dctor Id ");
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
            {
                _logger.LogError("Could not add availability slot. It might be overlapping");
                return Result<bool>.ErrorResult("Could not add availability slot. It might be overlapping or invalid.");
            }

                var slot = new AvailabilitySlot
                {
                    DoctorId = dto.DoctorId,
                    DayofWeek = dto.DayofWeek,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };
               var res =  await _repository.CreateAvaialbiltySlotAsync(slot);
            _logger.LogInformation("slot added succefully");
                return Result<bool>.SuccessResult(res, "slot added succfully");
            
        }

        public async Task<Result<List<GetAvailabilitySlotDto>>> GetBySlotDoctorIdAsync(int doctorId)
        {
            var isValidDoctor =await _repository.GetDoctorByIdAsync(doctorId);
            if(isValidDoctor == null)
            {
                _logger.LogError("Invalid dctor Id ");
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
                _logger.LogError("No slots found ");
                return Result<List<GetAvailabilitySlotDto>>.SuccessResult(avaialbalitySlotsDto, $"no avalibility slot found for {doctorId}");
            }
            _logger.LogInformation("availability slot fetche succefully");
            return Result<List<GetAvailabilitySlotDto>>.SuccessResult(avaialbalitySlotsDto, "avalibilty slot fetched succfully");
           
        }
    }
}
