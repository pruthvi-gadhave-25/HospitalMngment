using Azure;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class DepartmentService : IDepartmentService
    {   
        private readonly IDepartmentRepository _departmentRepo;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepo = departmentRepository;
        }
        public async  Task<Result<Department?>> AddDepartmentAsync(Department deprtment)
        {                                      
                var res =  await _departmentRepo.AddDepartmentAsync(deprtment);
                if(res == null)
                {
                    return Result<Department?>.ErrorResult("failed to add department");                    
                }
            return Result<Department?>.SuccessResult(res, "added suucefully");
        }



        public  async Task<Result<List<Department>>> GetDepartmentAsync()
        {
                            
            var res = await _departmentRepo.GetAllDepartmentsAsync();
            if (res == null)
            {
                return Result<List<Department>>.ErrorResult("failed to fetch departments");
            }
            return Result<List<Department>>.SuccessResult(res, "fetched  suucefully");
        }

        public async  Task<Result<Department?>> GetDepartmentByIdAsync(int id)
        {            
            var res = await _departmentRepo.GetDepartmentByIdAsync(id);
            if (res == null)
            {
                return Result<Department?>.ErrorResult("failed to get department or invalid id");
            }
            return Result<Department?>.SuccessResult(res, "fetched succefully");
        }

        public async Task<Result<bool>> UpdateDepartmentAsync(Department department)
        {
               var  dept = await _departmentRepo.GetDepartmentByIdAsync(department.Id);
            if(dept == null)
            {
                return Result<bool>.ErrorResult("department is null  or invalid id ");
            }
            return Result<bool>.SuccessResult(true ,"Updated deprtment ");
        }
        public async Task<Result<bool>> DeleteDepartmentAsync(int id)
        {
                var isdept = _departmentRepo.GetDepartmentByIdAsync(id);
                if (isdept == null)
                {
                    return Result<bool>.ErrorResult("invalid deot id");
                }
                var res =  await _departmentRepo.DeleteDepartmentAsync(id);
            return Result<bool>.SuccessResult(res, "deleted succesfullly");
        }
    }
}
