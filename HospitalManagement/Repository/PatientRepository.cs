using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagement.Repository
{
    public class PatientRepository : GenericRepository<Patient> , IPatientRepository
    {       
        public PatientRepository(AppDbContext context) :base(context)
        {
            
        }       

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            try
            {
                var res=  await _appDbContext.Patients
                    .Include( a=> a.Appointments)
                    .ThenInclude(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .FirstOrDefaultAsync( p => p.Id ==  id );
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
                var res =  await _appDbContext.Patients
                    .Include ( a=> a.Appointments)
                    .ThenInclude(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .ToListAsync() ;
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Patient>();
            }
        }

        public async Task<List<Patient>> SearchPatientAsync(string name , string mobileNo , string email)
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
                if(!string.IsNullOrWhiteSpace(email))
                {
                    query = query.Where(c => c.Email.Contains(email));
                }

                return await query.ToListAsync();
            }catch(Exception ex)
            {
                return null ;
            }
        }
    }
}
