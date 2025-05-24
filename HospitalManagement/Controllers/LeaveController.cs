using HospitalManagement.DTO;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController: ControllerBase
    {
        private readonly IServiceLeaveManagement _leaveService;

        public LeaveController(IServiceLeaveManagement serviceLeave)
        {
            _leaveService = serviceLeave;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLeaves()
        {
            var result = await _leaveService.GetPendingLeavesAsync();
            if(!result.IsSuccess)
            {
                return ApiResponseHelper.CreatFailure(result.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(result.Data, result.Message,200);
        }


        [HttpPost("add/leave")]
        public async Task<IActionResult> AddLeave( AddLeaveDto leave)
        {
            var result = await _leaveService.AddLeaveAsync(leave);
            if (!result.IsSuccess)
            {
                return ApiResponseHelper.CreatFailure(result.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(result.Data, result.Message, 200);
        }

        [HttpPut("approve/status/{leaveId}")]
        public async Task<IActionResult> UpdateLeaveStatus(int leaveId, LeaveStatus status, string approvedBy)
        {
            var result = await _leaveService.UpdateLeaveStatusAsync(leaveId, status, approvedBy);
            if (!result.IsSuccess)
            {
                return ApiResponseHelper.CreatFailure(result.Message, 404);
            }
            return ApiResponseHelper.CreateSuccess(result.Data, result.Message, 200);
        }
    }
}
