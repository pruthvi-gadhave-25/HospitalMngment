using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class LeaveService : IServiceLeaveManagement
    {
        //private readonly ILeaveRepository _leaveRepository;
        //private readonly IDoctorService _doctorService;

        private readonly LeaveManagementRepository _leaveRepository;
        private readonly DoctorRepository _doctorRepository;

        private readonly ILogger<LeaveService> _logger;

        public LeaveService(LeaveManagementRepository leaveRepo , DoctorRepository doctorRepository ,ILogger<LeaveService> logger)
        {
            _doctorRepository = doctorRepository; 
            _leaveRepository = leaveRepo;
            _logger = logger;
        }
        public async Task<Result<bool>> AddLeaveAsync(AddLeaveDto leave)
        {
            var doctor =  await _doctorRepository.GetById(leave.DoctorId);
            if(doctor == null)
            {
                _logger.LogError("doctor is not found");
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
            var res =  await _leaveRepository.Add(newLeaveDto);
            if(res ==  false)
            {
                _logger.LogError("Error occured while adding Leaved");
                return Result<bool>.ErrorResult("Error occured while adding Leave");
            }
            _logger.LogInformation("Leave Added succefully");
            return Result<bool>.SuccessResult(res, "Levae added succefully");
        }

        public async Task<Result<List<GetLeavesDto>>> GetPendingLeavesAsync()
        {
            var pendingLeaves = await _leaveRepository.GetPendingLeavesAsync();

            if (pendingLeaves == null || pendingLeaves.Count()== 0)
            {
                _logger.LogError("No pending leaves found " );
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

            _logger.LogInformation("leaves fetched succefully");
            return Result<List<GetLeavesDto>>.SuccessResult(newLeaves, "Pending leaves fetched successfully.");
        }

        public async  Task<Result<bool>> UpdateLeaveStatusAsync(int leaveId, LeaveStatus status, string approvedBy)
        {
            var updated = await _leaveRepository.UpdateLeaveStatusAsync(leaveId, status, approvedBy);
            if (!updated)
            {
                _logger.LogError("Leave not found or update failed");
                return Result<bool>.ErrorResult("Leave not found or update failed");
            }
            _logger.LogInformation("Updated Succefully");
            return Result<bool>.SuccessResult(true, $"Leave updated {status} successfully");
        }


    }
}
