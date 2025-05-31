using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Numerics;

namespace HospitalManagement.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> BookAppointment(Appointment appointment)
        {
            try
            {
                var res = await _context.AddAsync(appointment);
                await _context.SaveChangesAsync();
                return true;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> CancelAppointment(int appointmentId)
        {
            try
            {   
              var appointment=   await _context.Appointments.FindAsync(  appointmentId );
                if (appointment == null)
                {
                    return false;
                }
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<Appointment>> GetAllAppointmentAsync()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .Include(p => p.Patient)
                    .ToListAsync();
             
                return appointments ?? new List<Appointment>();
            }
            catch (Exception ex)
            {
                //log edexception 

                return new List<Appointment>();
            }
        }

        public async Task<List<Appointment>> GetAppointmentBtDoctorAsync(int doctorId)
        {
            try
            {
                var  appointments =await   _context.Appointments
                    .Include( d => d.Doctor)
                    .ThenInclude( d=> d.Department)
                    .Where(a => a.DoctorId == doctorId).ToListAsync();

                if(appointments == null) 
                    return null;

                return appointments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Appointment>> GetAppointmentByDateAsync(DateTime date)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.AppointmentDate == date).ToListAsync();

                if (appointments == null)
                    return null;

                return appointments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Task<Appointment> GetAppointmentAsync(int id)
        {
            try
            {
                var appointment =  _context.Appointments.FirstOrDefaultAsync( p => p.Id == id );
                if (appointment == null)
                    return null;

                return appointment;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> RescheduleAppointment(int appointmentId, DateTime newDate, TimeOnly newTime)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(appointmentId);
                if(appointment == null)
                {
                    return false;
                }
                appointment.AppointmentDate = newDate;
                appointment.AppointmentTime = newTime;

                 _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<Appointment>> GetDailyAppointmetsByDoctor(int doctorId , DateTime date , int pageIndex, int pageSize)
        {
            try
            { //returns appointmets having same date of that doctor 
               
                var skip = (pageIndex - 1) * pageSize;

                var apppointments = await _context.Appointments
                    .Include(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(d => d.DoctorId == doctorId && d.AppointmentDate == date)
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                if (apppointments == null)
                {
                    return null;
                }
                return apppointments;
                var appointments = await _context.Appointments
                    .AsNoTracking()
                    .Include(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a => a.DoctorId == doctorId  && a.AppointmentDate == date )
                    .Skip( (pageIndex -1)* pageSize )
                    .ToListAsync();

                if (appointments == null)
                    return null;

                return appointments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Appointment>> GetDailyAppointmetsByDepartment(int departmentId, DateTime date , int pageIndex , int pageSize)
        {
            try
            {   
                var skip = (pageIndex -1 ) * pageSize;  

                var apppointments =  await _context.Appointments
                    .Include(d => d.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where( d => d.Doctor.DepartmentId ==  departmentId && d.AppointmentDate ==  date)
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                if(apppointments == null ||  apppointments.Count ==0 )
                {
                    return null;
                }
                return apppointments;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async  Task<bool> UpdateAppointentAsync(int appointmentId)
        {
            try
            {   
                var appointment = await _context.Appointments.FirstOrDefaultAsync(d => d.Id == appointmentId);  
                if(appointment == null)
                {
                    return false;
                }
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine();
                return false;
            }
        }

        public async Task<List<Appointment>> GetPatientVisitCountAsync(int patientId)
        {
            try
            {                   
                 var appointments =await   _context.Appointments
                    .Where(p => p.PatientID == patientId && p.Status == Helpers.AppointmentStatus.Completed )                   
                    .AsNoTracking()
                    .ToListAsync();

                if(appointments == null || appointments.Count == 0)
                {
                    return null;
                }

                return appointments;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null ;
            }
        }
    }
}
