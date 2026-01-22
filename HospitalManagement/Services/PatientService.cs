using AutoMapper;
using Azure;
using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Interface;
using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HospitalManagement.Services
{
    public class PatientService :  IPatientService
    {
        private readonly PatientRepository _patientRepo;
        private readonly ILogger<LoggingActionFilter> _logger;
        private readonly IMapper _mapper; 
        private readonly IMemoryCache _cache;  

        
        public PatientService(PatientRepository patientRepository  ,ILogger<LoggingActionFilter> logger  , IMapper mapper  , IMemoryCache memoryCache)
        {
            _patientRepo = patientRepository;
            _logger = logger;
            _mapper = mapper;
            _cache = memoryCache;
        }

        public async Task<bool> AddPatientAsync(PatientAddDto patientDto)
        {           
            try
            {
                if(patientDto == null)
                {
                    return false;
                }

                var patient = new Patient
                { 
                    Name = patientDto.Name,
                    Email = patientDto.Email,
                    Mobile = patientDto.Mobile,
                    Gender = patientDto.Gender,
                    Dob = patientDto.Dob
                };                
               await _patientRepo.Add(patient);
               await _patientRepo.SaveAsync();
                return true;

            }catch (Exception ex)
            {
                _logger.LogError($"error occured {ex.Message}");
                return false;
            }
        }

        public async Task<Result<GetPatientDto>> GetPatientByIdAsync(int id)
        {           
            if (id == null)
            {
                return null;
            }
            var res = await _patientRepo.GetPatientByIdAsync(id);     // get realted data
                   
            if(res  == null)
            {
                _logger.LogError("Patient Not found");
                return Result<GetPatientDto>.ErrorResult("patient not found");
            }
            var patient = new GetPatientDto
            {
                Id = res.Id,
                Name = res.Name,
                Email = res.Email,
                Mobile = res.Mobile,
                Gender = res.Gender,
                Dob = res.Dob,
                Appointments = res.Appointments?.Select(p => new GetAppointmentsDto
                {
                    Diagnoasis = p.Diagnoasis,
                    Treatement = p.Treatement,
                    Medications = p.Medications,
                    DepartmentName = p.Doctor?.Department?.Name?? "N/A",
                    DoctorName = p.Doctor?.Name ?? "Unknown Doctor"
                }).ToList(),
            };
            return Result<GetPatientDto>.SuccessResult(patient , "patient fetched succsfully");            
        }

        // imeplemnts in memory caching in this //   and  automapper 
        public async  Task<Result<List<GetPatientDto>>> GetPatientsAsync()
        {
            try{

                string cacheKey = "Patientslist";

                if(_cache.TryGetValue(cacheKey,out List<GetPatientDto> cachedPatients))
                {
                    return Result<List<GetPatientDto>>.SuccessResult(cachedPatients , "fethced succefully");
                }

                //var patients =res.Select( p => new GetPatientDto
                //{
                //    Id =p.Id,
                //    Name = p.Name,
                //    Email = p.Email,
                //    Mobile = p.Mobile,
                //    Gender = p.Gender,
                //    Dob = p.Dob,

                //    Appointments = p.Appointments?.Select( p => new GetAppointmentsDto
                //    {
                //        Diagnoasis = p.Diagnoasis,
                //        Treatement = p.Treatement,
                //        Medications = p.Medications,
                //        AppointmentDate = p.AppointmentDate,
                //        AppointmentTime = p.AppointmentTime,
                //        DoctorName = p.Doctor?.Name ?? "N/a",
                //        DepartmentName = p.Doctor?.Department?.Name ?? "N/a"

                //    }).ToList() ?? new(),
                //}).ToList();

                var res = await _patientRepo.GetPatientsAsync();

                if (res == null  && !res.Any())
                {
                    return Result<List<GetPatientDto>>.ErrorResult("No patients found");
                }
                var patientsDto =  _mapper.Map<List<GetPatientDto>>(res);


                var options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));


                    _cache.Set(cacheKey, patientsDto ,options);

                return Result<List<GetPatientDto>>.SuccessResult( patientsDto,"patients fetched succefully"); ;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Occured {ex.Message}");
                return  Result<List<GetPatientDto>>.ErrorResult("No patients found");
            }
        }

        public async Task<Result<List<Patient>>> SearchPatientsAsync(string?  name, string? email, string? mobileNo)
        {
            try{
               var patients =  await _patientRepo.SearchPatientAsync(name, email, mobileNo);
               
                return Result<List<Patient>>.SuccessResult( patients,"patient found succefully"); ;
               
            }catch(Exception ex)
            {
                _logger.LogError($"Error occured {ex.Message}");
                return Result<List<Patient>>.ErrorResult("No patients found"); ;
            }
        }
    }
}
