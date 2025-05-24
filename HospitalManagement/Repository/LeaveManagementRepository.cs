using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class LeaveManagementRepository : ILeaveRepository
    {

        private readonly AppDbContext _context;
        public LeaveManagementRepository(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<bool> AddLeaveAsync(LeaveManagment leave)
        {
            try
            {

                var res = _context.LeaveManagments.Add(leave);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
          
        }

        public async  Task<List<LeaveManagment>> GetPendingLeavesAsync()
        {
            try
            {
                return await _context.LeaveManagments.Where(l => l.Status == LeaveStatus.Pending).ToListAsync();

            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<LeaveManagment>();
            }
        }

        public async Task<bool> IsDoctorOnLeaveAsync(int doctorId, DateTime appointmentDate)
        {
            try
            {
                return await _context.LeaveManagments.AnyAsync(l =>
                    l.DoctorId == doctorId &&
                    (int)l.Status == (int)LeaveStatus.Approved &&
                    appointmentDate >= l.LeaveStartDate &&
                    appointmentDate <= l.LeaveEndDate
                );
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        
        }

        public async  Task<bool> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status, string approvedBy)
        {
            try
            {
                var leave = await _context.LeaveManagments.FindAsync(leaveId);
                if (leave == null) return false;

                leave.Status = status;
                leave.IsApproved = (status == LeaveStatus.Approved);
                leave.ApprovedBy = approvedBy;

                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
