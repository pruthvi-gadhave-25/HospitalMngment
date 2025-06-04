using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IServiceLeaveManagement
    {
        Task<Result<bool>> AddLeaveAsync(AddLeaveDto leaveDto);
        Task<Result<List<GetLeavesDto>>> GetPendingLeavesAsync();
        Task<Result<bool>> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status, string approvedBy);
    }
}
