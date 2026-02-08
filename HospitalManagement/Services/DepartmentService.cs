using Azure;
using HospitalManagement.Data.UnitOfWork;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Services
{
    public class DepartmentService : IDepartmentService
    {   
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Department?>> AddDepartmentAsync(Department deprtment)
        {
            var res = await _unitOfWork.DepartmentRepository.Add(deprtment);
            if (res == true)
            {
                return Result<Department?>.ErrorResult("failed to add department");                    
            }
            await _unitOfWork.SaveChangesAsync();
            return Result<Department?>.SuccessResult(deprtment, "added suucefully");
        }

        public async Task<Result<List<Department>>> GetDepartmentAsync()
        {
            var res = await _unitOfWork.DepartmentRepository.GetAll();
            if (res == null)
            {
                return Result<List<Department>>.ErrorResult("failed to fetch departments");
            }
            return Result<List<Department>>.SuccessResult(res.ToList(), "fetched  suucefully");
        }

        public async Task<Result<Department?>> GetDepartmentByIdAsync(int id)
        {            
            var res = await _unitOfWork.DepartmentRepository.GetById(id);
            if (res == null)
            {
                return Result<Department?>.ErrorResult("failed to get department or invalid id");
            }
            return Result<Department?>.SuccessResult(res, "fetched succefully");
        }

        public async Task<Result<bool>> UpdateDepartmentAsync(Department department)
        {
            var dept = await _unitOfWork.DepartmentRepository.GetById(department.Id);
            if (dept == null)
            {
                return Result<bool>.ErrorResult("department is null  or invalid id ");
            }
            await _unitOfWork.DepartmentRepository.Update(department);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.SuccessResult(true, "Updated deprtment ");
        }

        public async Task<Result<bool>> DeleteDepartmentAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(id);
            if (department == null)
            {
                return Result<bool>.ErrorResult("invalid deot id");
            }
            var res = await _unitOfWork.DepartmentRepository.Delete(department);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.SuccessResult(res, "deleted succesfullly");
        }
    }
}
