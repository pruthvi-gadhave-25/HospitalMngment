using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    //[Authorize (Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeprtmentController : ControllerBase
    {   
        private readonly IDepartmentService _deptService;

        public DeprtmentController(IDepartmentService departmentService)
        {
         _deptService = departmentService;   
        }

        [Authorize(Roles = "Doctor,Receptionist")]
        [HttpGet("get/departments")]
        public async  Task<IActionResult> GetDepartments()
        {
                var res = await _deptService.GetDepartmentAsync();

                if (res == null)
                {
                    return ApiResponseHelper.CreatFailure(res.Message, 400);
                }

                return ApiResponseHelper.CreateSuccess(res.Data,res.Message);
            
        }



        [HttpPost("add/department")]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
                var res =  await _deptService.AddDepartmentAsync(department);  
                if (res == null)
                {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);


        }

        [HttpPut("update/department")]
        public async Task<IActionResult> UpdateDepartment(Department department)
        { 
            if(department == null)
            {
                throw new ArgumentNullException("department is not valid");
            }

             var res = await _deptService.UpdateDepartmentAsync(department);

            if(res.IsSuccess ==  false ) 
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }

            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);

        }

        [HttpDelete("delete/department/{id}")]
        public async Task<IActionResult> DeleteDepartmentById(int id)
        {
                var validDept = await _deptService.GetDepartmentByIdAsync(id);
                if(validDept == null)
                {
                    return ApiResponseHelper.CreatFailure(validDept.Message, 400);
                }
                var res = await _deptService.DeleteDepartmentAsync(id);
                if (res.IsSuccess == false)
                {
                    return ApiResponseHelper.CreatFailure(res.Message, 400);
                }
                return ApiResponseHelper.CreateSuccess(res.Data, res.Message);
            
        }

        [Authorize(Roles = "Doctor,Receptionist")]
        [HttpGet("get/deparment/{id}")]
        public async Task<IActionResult> getDepartmentByIdAsync(int id)
        {
            var res = await _deptService.GetDepartmentByIdAsync(id);

            if(res == null)
            {
                return ApiResponseHelper.CreatFailure(res.Message, 400);
            }
            return ApiResponseHelper.CreateSuccess(res.Data, res.Message);

        }
    }
}
