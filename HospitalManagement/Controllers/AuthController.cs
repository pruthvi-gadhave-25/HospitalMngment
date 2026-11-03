using HospitalManagement.DTO;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Services;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;

        public AuthController(IAuthService authservice, IOtpService otpService)
        {
            _authService = authservice;
            _otpService = otpService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisteUser(RegisterRequestDto registerRequestDto)
        {
            try
            {
                var res = await _authService.RegisterUser(registerRequestDto);
                if (res == null)
                {
                    return NotFound(res);
                }
                //_logger.LogInformation($"User logged in succefully {res.Message}");
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginRequestDto loginRequestDto)
        {
            try
            {
                var res = await _authService.LoginUserASync(loginRequestDto);
                if (res == null)
                {

                    return Unauthorized(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("otp/generate")]
        public async Task<IActionResult> GenerateOtp([FromBody] GenerateOtpRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            try
            {
                await _otpService.GenerateOtpAsync(request.UserId, request.Purpose);

                return Ok(new
                {
                    Message = "OTP generated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("otp/verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var isVerified = await _otpService.VerifyOtpAsync(request.UserId, request.Purpose, request.Otp);

            if (!isVerified)
            {
                return BadRequest(new
                {
                    Message = "Invalid or expired OTP."
                });
            }

            return Ok(new
            {
                Message = "OTP verified successfully."
            });
        }
    }
}
