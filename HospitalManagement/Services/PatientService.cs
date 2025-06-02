using Azure;
using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Interface;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Services
{
    public class PatientService : IPatientService
    {   
        private readonly IPatientRepository _patientRepository;
        private readonly IRepository<Patient> _repository;
        private readonly AppDbContext _appDbContext;
        public PatientService(IPatientRepository patientRepo ,
            IRepository<Patient> repository
            , AppDbContext appDbContext
            )
        {
            _patientRepository = patientRepo;
            _repository = repository;
            _appDbContext = appDbContext;
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
               await _repository.Add(patient);
               await _repository.SaveAsync();
                return true;

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
            //var res = await _patientRepository.GetPatientByIdAsync(id);

            var res = await _appDbContext.Patients
                  .AsNoTracking()
                .Include(a => a.Appointments)
                .ThenInclude(a => a.Doctor)
                .ThenInclude(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id);       //calling related entries here /chaged beacuse          
            //var res = await _repository.GetById(id); //new generic repos
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
                           
                var res =  await _appDbContext.Patients
                    .AsNoTracking()
                    .Include(a => a.Appointments)
                    .ThenInclude(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .ToListAsync();
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

        public Task<List<Patient>> SearchPatientsAsync(string?  name, string? email, string? mobileNo)
        {
            try{
                var query = _appDbContext.Patients.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(c => c.Name.Contains(name));
                }
                if (!string.IsNullOrWhiteSpace(mobileNo))
                {
                    query = query.Where(c => c.Mobile.Contains(mobileNo));
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    query = query.Where(c => c.Email.Contains(email));
                }
                var res = query.ToListAsync();
                return res;
                //return _patientRepository.SearchPatientAsync(name, email, mobileNo);
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
