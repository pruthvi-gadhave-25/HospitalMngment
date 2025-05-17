using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeprtmentController : ControllerBase
    {   
        private readonly IDepartmentService _deptService;

        public DeprtmentController(IDepartmentService departmentService)
        {
         _deptService = departmentService;   
        }


        [HttpGet("getDepartments")]
        public async  Task<IActionResult> GetDeprtments()
        {
            try
            {
                var res = await _deptService.GetDepartmentAsync();

                if (res == null)
                {
                    return NotFound("No departments found.");
                }

                return Ok(res);
            }
            catch (Exception ex)
            {   //logexception 
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while get  departments.");
            }
        }



        [HttpPost("addDepartment")]
        public async Task<IActionResult> Createdeprtment(Department department)
        {
            try
            {
                var res =  await _deptService.AddDepartmentAsync(department);  
                if (res == null)
                {
                    return NotFound("invalid data");
                }
                return Ok("Added Successfully");
            }
            catch(Exception ex)
            {
                //log 
                return StatusCode(500, "Error occured while adding Department"); 
            }
        }
    }
}
