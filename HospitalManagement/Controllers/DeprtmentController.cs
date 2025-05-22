using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Authorize (Roles ="Admin, Doctor, Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeprtmentController : ControllerBase
    {   
        private readonly IDepartmentService _deptService;

        public DeprtmentController(IDepartmentService departmentService)
        {
         _deptService = departmentService;   
        }


        [HttpGet("get/departments")]
        public async  Task<IActionResult> GetDepartments()
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



        [HttpPost("add/department")]
        public async Task<IActionResult> CreateDepartment(Department department)
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

        [HttpPut("update/department")]
        public async Task<IActionResult> UpdateDepartment(Department department)
        {
            try
            {   
                if(department == null)
                {
                    return BadRequest("invalid data");
                }
                var res = await _deptService.UpdateDepartmentAsync(department);
                if(res ==  false ) 
                { 
                    return BadRequest("invalid data or Id "); 
                }
                return Ok("Updated succefully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("not updated ");
            }
        }

        [HttpDelete("delete/department/{id}")]
        public async Task<IActionResult> DeleteDepartmentById(int id)
        {
            try
            {
                var validDept = await _deptService.GetDepartmentByIdAsync(id);
                if(validDept == null)
                {
                    return BadRequest("Invlaid department");
                }
                var res = await _deptService.DeleteDepartmentAsync(id);
                if (res == false)
                {
                    return BadRequest("invalid id ");
                }
                return Ok("deleted succefully");

            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
                return BadRequest("invalid id ");
            }
        }


        [HttpGet("get/deparment/{id}")]
        public async Task<IActionResult> getDepartmentByIdAsync(int id)
        {
            try
            {
                var res = await _deptService.GetDepartmentByIdAsync(id);

                if(res == null)
                {
                    return NotFound("Not found");
                }
                return Ok(res);
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("id is invalid");
            }
        }
    }
}
