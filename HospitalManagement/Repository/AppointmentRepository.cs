using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
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
                var appointments = await _context.Appointments.ToListAsync();

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
                var  appointments =await   _context.Appointments.Where(a => a.DoctorId == doctorId).ToListAsync();
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
                var appointments = await _context.Appointments.Where(a => a.AppointmentDate == date).ToListAsync();
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
    }
}
