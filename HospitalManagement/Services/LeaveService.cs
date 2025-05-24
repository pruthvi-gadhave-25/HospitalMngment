using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class LeaveService : IServiceLeaveManagement
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IDoctorService _doctorService;

        public LeaveService(ILeaveRepository leaveRepo , IDoctorService doctorService)
        {
            _doctorService = doctorService; 
            _leaveRepository = leaveRepo;   
        }
        public async Task<Result<bool>> AddLeaveAsync(AddLeaveDto leave)
        {
            var doctor =  await _doctorService.GetDoctorByIdAsync(leave.DoctorId);
            if(doctor == null)
            {
                return Result<bool>.ErrorResult("docotor is invalid");
            }

            var newLeaveDto = new LeaveManagment
            {
                DoctorId = leave.DoctorId,
                LeaveEndDate = leave.LeaveStartDate,
                LeaveStartDate = leave.LeaveEndDate,
                Status =  LeaveStatus.Pending,
                IsApproved = false ,
                ApprovedBy = string.Empty
            };
            var res =  await _leaveRepository.AddLeaveAsync(newLeaveDto);
            if(res ==  false)
            {
                return Result<bool>.ErrorResult("erro occured while adding Leave");
            }
            return Result<bool>.SuccessResult(res, "Levae added succefully");
        }

        public async Task<Result<List<GetLeavesDto>>> GetPendingLeavesAsync()
        {
            var pendingLeaves = await _leaveRepository.GetPendingLeavesAsync();

            if (pendingLeaves == null || pendingLeaves.Count()== 0)
            {
                return Result<List<GetLeavesDto>>.ErrorResult("No pending leave requests found.");
            }
            var newLeaves = pendingLeaves.Select(p => new GetLeavesDto
            {
                Id = p.Id,
                DoctorId = p.DoctorId,
                LeaveStartDate = p.LeaveStartDate,
                LeaveEndDate = p.LeaveEndDate,
                Status = p.Status,
                IsApproved = p.IsApproved,
                ApprovedBy = p.ApprovedBy,
            }).ToList();

            return Result<List<GetLeavesDto>>.SuccessResult(newLeaves, "Pending leaves fetched successfully.");
        }

        public async  Task<Result<bool>> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status, string approvedBy)
        {
            var updated = await _leaveRepository.UpdateLeaveStatusAsync(leaveId, status, approvedBy);
            if (!updated)
                return Result<bool>.ErrorResult("Leave not found or update failed");

            return Result<bool>.SuccessResult(true, $"Leave updated {status} successfully");
        }


    }
}
