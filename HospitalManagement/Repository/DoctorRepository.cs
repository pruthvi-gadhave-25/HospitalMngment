using HospitalManagement.Data;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Migrations;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class DoctorRepository : GenericRepository<Doctor> , IDoctorRepository
    {   
     
        public DoctorRepository(AppDbContext appDbContext) : base(appDbContext) 
        {        
           
        }
        public async Task<Doctor?> AddDoctorAsync(Doctor doctor)
        {
            try
            {
                if (doctor == null)
                {
                    return null;
                }
                var res = await _appDbContext.Doctors.AddAsync(doctor);

                await _appDbContext.SaveChangesAsync();
                return res.Entity;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

       

        public async  Task<bool> DeleteDoctorAsync(int id)
        {
            try
            {
                var doctor = await _appDbContext.Doctors.FirstOrDefaultAsync(p => p.Id == id);

                if (doctor == null)
                {
                    return false;
                }
                var res = _appDbContext.Doctors.Remove(doctor);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async  Task<List<Doctor>> GetAllDocotorsAsync()
        {
            try
            {
                var doctors = await _appDbContext.Doctors.
                     Include(d => d.Department)
                    .Include(a => a.AvailabilitySlots)
                    .ToListAsync();

                return doctors ?? new List<Doctor>();
            }
            catch (Exception ex)
            {
                //log edexception 

                return new List<Doctor>();
            }
        }
      
        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            try
            {
                var doctor = await _appDbContext.Doctors.
                    Include(d => d.Department)
                    .Include( a => a.AvailabilitySlots)
                    .FirstOrDefaultAsync( d  => d.Id == id);
                if (doctor == null)
                {
                    return null;
                }
                return doctor;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                _appDbContext.Doctors.Update(doctor);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"exception occurs {e.Message}");

                return false;
            }
        }

        public async Task<bool> CreateAvaialbiltySlotAsync(AvailabilitySlot slot)
        {
            try
            {
                await _appDbContext.AvailabilitySlots.AddAsync(slot);
                await _appDbContext.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<AvailabilitySlot>> GetBySlotDoctorIdAsync(int doctorId)
        {
            try
            {
           
             var res =  await _appDbContext.AvailabilitySlots.Where(d => d.DoctorId == doctorId).ToListAsync();                
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<AvailabilitySlot> { };
            }
        }
    }
}
