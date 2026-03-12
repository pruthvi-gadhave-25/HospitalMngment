using AutoMapper;
using HospitalManagement.Data.UnitOfWork;
using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PatientService> _logger; 
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public PatientService(IUnitOfWork unitOfWork, ILogger<PatientService> logger, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _cache = memoryCache;
        }

        public async Task<bool> AddPatientAsync(PatientAddDto patientDto)
        {
            try
            {
                if (patientDto == null)
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

                await _unitOfWork.PatientRepository.Add(patient);
                await _unitOfWork.SaveChangesAsync();

                // invalidate cache
                InvalidatePatientCache();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error occured {ex.Message}");
                return false;
            }
        }

        public async Task<Result<GetPatientDto>> GetPatientByIdAsync(int id)
        {
            var res = await _unitOfWork.PatientRepository.GetPatientByIdAsync(id); // get related data

            if (res == null)
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
                    DepartmentName = p.Doctor?.Department?.Name ?? "N/A",
                    DoctorName = p.Doctor?.Name ?? "Unknown Doctor"
                }).ToList(),
            };

            return Result<GetPatientDto>.SuccessResult(patient, "patient fetched succsfully");
        }

        public async Task<Result<PagedResult<GetPatientDto>>> GetPatientsAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                //string cacheKey = $"Patientslist_page_{pageIndex}_size_{pageSize}";

                //if (_cache.TryGetValue(cacheKey, out PagedResult<GetPatientDto> cachedPatients))
                //{
                //    return Result<PagedResult<GetPatientDto>>
                //        .SuccessResult(cachedPatients, "patients fetched from cache");
                //}

                var allPatients = await _unitOfWork.PatientRepository.GetPatientsAsync();

                if (allPatients == null || !allPatients.Any())
                {
                    return Result<PagedResult<GetPatientDto>>
                        .ErrorResult("No patients found");
                }

                int totalCount = allPatients.Count;
                int skip = (pageIndex - 1) * pageSize;

                var paginatedPatients = allPatients
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var patientsDto = _mapper.Map<List<GetPatientDto>>(paginatedPatients);

                var pagedResult = new PagedResult<GetPatientDto>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Data = patientsDto
                };

                var options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                //_cache.Set(cacheKey, pagedResult, options);

                return Result<PagedResult<GetPatientDto>>
                    .SuccessResult(pagedResult, "patients fetched successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");

                return Result<PagedResult<GetPatientDto>>
                    .ErrorResult("Error fetching patients");
            }
        }

        public async Task<Result<List<Patient>>> SearchPatientsAsync(string? name, string? email, string? mobileNo)
        {
            try
            {
                var patients = await _unitOfWork.PatientRepository.SearchPatientAsync(name, mobileNo, email);

                return Result<List<Patient>>.SuccessResult(patients, "patient found succefully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured {ex.Message}");
                return Result<List<Patient>>.ErrorResult("No patients found");
            }
        }

        public async Task<Result<bool>> UpdatePatientAsync(UpdatePatientDto updateDto)
        {
            try
            {
                if (updateDto == null)
                    return Result<bool>.ErrorResult("Invalid data");

                var existing = await _unitOfWork.PatientRepository.GetById(updateDto.Id);
                if (existing == null)
                    return Result<bool>.ErrorResult("Patient not found");

                existing.Name = updateDto.Name;
                existing.Email = updateDto.Email;
                existing.Mobile = updateDto.Mobile;
                existing.Gender = updateDto.Gender;
                existing.Dob = updateDto.Dob;

                await _unitOfWork.PatientRepository.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                // invalidate cache
                InvalidatePatientCache();

                return Result<bool>.SuccessResult(true, "Patient updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured {ex.Message}");
                return Result<bool>.ErrorResult("Failed to update patient");
            }
        }

        public async Task<Result<bool>> DeletePatientAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return Result<bool>.ErrorResult("Invalid patient ID");

                var existing = await _unitOfWork.PatientRepository.GetById(id);
                if (existing == null)
                    return Result<bool>.ErrorResult("Patient not found");

                await _unitOfWork.PatientRepository.Delete(existing);
                await _unitOfWork.SaveChangesAsync();

                // invalidate cache
                InvalidatePatientCache();

                _logger.LogInformation($"Patient with ID {id} deleted successfully");
                return Result<bool>.SuccessResult(true, "Patient deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting patient: {ex.Message}");
                return Result<bool>.ErrorResult("Failed to delete patient");
            }
        }

        private void InvalidatePatientCache()
        {
            _cache.Remove("Patientslist");
        }
    }
}
