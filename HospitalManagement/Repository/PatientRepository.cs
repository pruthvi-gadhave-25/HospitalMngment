using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddPatientAsync(Patient patient)
        {
            try
            {
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            try
            {
                var res=  await _context.Patients.FindAsync(id);
                if(res == null)
                {
                    return null;
                }
                return res;
            }catch(Exception ex)
            {
                Console.WriteLine( ex.Message);
                return null;
            }
        }

        public async  Task<List<Patient>> GetPatientsAsync()
        {
            try
            {
                var res =  await _context.Patients.ToListAsync();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Patient>();
            }
        }
    }
}
