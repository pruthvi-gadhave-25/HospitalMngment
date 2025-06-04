using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface ILeaveRepository
    {
        Task<List<LeaveManagment>> GetPendingLeavesAsync();
        Task<bool> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status, string approvedBy);
        Task<bool> IsDoctorOnLeaveAsync(int doctorId, DateTime appointmentDate);
    }
}
