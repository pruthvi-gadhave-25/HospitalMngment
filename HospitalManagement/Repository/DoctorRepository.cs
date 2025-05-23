using HospitalManagement.Data;
using HospitalManagement.DTO.AvailabiltyDto;
using HospitalManagement.Migrations;
using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class DoctorRepository : IDoctorRepository
    {   
        private readonly AppDbContext _context;


        public DoctorRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Doctor?> AddDoctorAsync(Doctor doctor)
        {
            try
            {
                if (doctor == null)
                {
                    return null;
                }
                var res = await _context.Doctors.AddAsync(doctor);

                await _context.SaveChangesAsync();
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
                var doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.Id == id);

                if (doctor == null)
                {
                    return false;
                }
                var res = _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
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
                var doctors = await _context.Doctors.
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
                var doctor = await _context.Doctors.
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
                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync();
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
                await _context.AvailabilitySlots.AddAsync(slot);
                await _context.SaveChangesAsync();
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
           
             var res =  await _context.AvailabilitySlots.Where(d => d.DoctorId == doctorId).ToListAsync();                
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
