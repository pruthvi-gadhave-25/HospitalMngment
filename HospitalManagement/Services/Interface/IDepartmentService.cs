using HospitalManagement.Helpers;
using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IDepartmentService
    {
        Task<Result<List<Department>>> GetDepartmentAsync();
        Task<Result<bool>> UpdateDepartmentAsync(Department department);
        Task<Result<bool>> DeleteDepartmentAsync(int id);
        Task<Result<Department?>> GetDepartmentByIdAsync(int id);
        Task<Result<Department?>> AddDepartmentAsync(Department deprtment);
    }
}
