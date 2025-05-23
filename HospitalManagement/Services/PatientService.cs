using Azure;
using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class PatientService : IPatientService
    {   
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepo)
        {
            _patientRepository = patientRepo;
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
                var res = await _patientRepository.AddPatientAsync(patient);
                return res;

            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Result<GetPatientDto>> GetPatientByIdAsync(int id)
        {           
            if (id == null)
            {
                return null;
            }
            var res = await _patientRepository.GetPatientByIdAsync(id);
            if(res  == null)
            {
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

        public async  Task<List<GetPatientDto>> GetPatientsAsync()
        {
            try{ 
                           
                var res = await _patientRepository.GetPatientsAsync();
                var patients =res.Select( p => new GetPatientDto
                {
                    Id =p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    Mobile = p.Mobile,
                    Gender = p.Gender,
                    Dob = p.Dob,
                    
                    Appointments = p.Appointments?.Select( p => new GetAppointmentsDto
                    {
                        Diagnoasis = p.Diagnoasis,
                        Treatement = p.Treatement,
                        Medications = p.Medications,
                        AppointmentDate = p.AppointmentDate,
                        AppointmentTime = p.AppointmentTime,
                        DoctorName = p.Doctor?.Name ?? "N/a",
                        DepartmentName = p.Doctor?.Department?.Name ?? "N/a"

                    }).ToList() ?? new(),
                }).ToList();
                return patients;

            }
            catch (Exception ex)
            {
                return  null;
            }
        }

        public Task<List<Patient>> SearchPatientsAsync(string?  name, string? email, string? mobile)
        {
            try{
                return _patientRepository.SearchPatientAsync(name, email, mobile);
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
