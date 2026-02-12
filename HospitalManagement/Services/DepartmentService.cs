using Azure;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class DepartmentService : IDepartmentService
    {   
       private readonly DepartmentRepository _departmentRepo;


        public DepartmentService(DepartmentRepository departmentRepository)
        {
            _departmentRepo = departmentRepository;
        }
        public async  Task<Result<Department?>> AddDepartmentAsync(Department deprtment)
        {
           var res =  await _departmentRepo.Add(deprtment);
                if(res == true )
                {
                    return Result<Department?>.ErrorResult("failed to add department");                    
                }
            return Result<Department?>.SuccessResult(deprtment,"added suucefully");
        }


        public  async Task<Result<List<Department>>> GetDepartmentAsync()
        {
                            
            var res = await _departmentRepo.GetAll();
            if (res == null)
            {
                return Result<List<Department>>.ErrorResult("failed to fetch departments");
            }
            return Result<List<Department>>.SuccessResult(res.ToList(), "fetched  suucefully");
        }

        public async  Task<Result<Department?>> GetDepartmentByIdAsync(int id)
        {            
            var res = await _departmentRepo.GetById(id);
            if (res == null)
            {
                return Result<Department?>.ErrorResult("failed to get department or invalid id");
            }
            return Result<Department?>.SuccessResult(res, "fetched succefully");
        }

        public async Task<Result<bool>> UpdateDepartmentAsync(Department department)
        {
               var  dept = await _departmentRepo.GetById(department.Id);
            if(dept == null)
            {
                return Result<bool>.ErrorResult("department is null  or invalid id ");
            }
            await _departmentRepo.Update(department);
            return Result<bool>.SuccessResult(true ,"Updated deprtment ");
        }
        public async Task<Result<bool>> DeleteDepartmentAsync(int id)
        {
                var department =await  _departmentRepo.GetById(id);
                if (department == null)
                {
                    return Result<bool>.ErrorResult("invalid deot id");
                }
                var res =  await _departmentRepo.Delete(department);
            return Result<bool>.SuccessResult(res, "deleted succesfullly");
        }
    }
}
