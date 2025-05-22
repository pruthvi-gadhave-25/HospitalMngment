using HospitalManagement.DTO;
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

        public AuthController(IAuthService authservice)
        {
            _authService = authservice;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisteUser(RegisterRequestDto registerRequestDto)
        {
            try
            {
                var res =  await _authService.RegisterUser(registerRequestDto);
                if (res == null)
                {
                    return NotFound(res);
                }
                return Ok(res);
            }catch (Exception ex)
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
                var res =await   _authService.LoginUserASync(loginRequestDto);
                if(res == null)
                {
                    return NotFound(res);
                }
                return Ok(res); 
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
