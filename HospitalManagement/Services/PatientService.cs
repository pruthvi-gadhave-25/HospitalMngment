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
        public async Task<bool> AddPatientAsync(Patient patient)
        {
            try
            {
               if(patient == null)
                {
                    return false;
                }
                var res = await _patientRepository.AddPatientAsync(patient);
                return res;

            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            try
            {
                if (id == null)
                {
                    return null;
                }
                var res = await _patientRepository.GetPatientByIdAsync(id);
                return res;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<List<Patient>> GetPatientsAsync()
        {
            try{ 
                           
                var res = await _patientRepository.GetPatientsAsync();
                return res;

            }
            catch (Exception ex)
            {
                return  null;
            }
        }
    }
}
