using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await  _userService.GetUsersAsync();

                return Ok(users);   
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
