using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartmentAsync();
        Task<bool> UpdateDepartmentAsync(Department department);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<Department?> AddDepartmentAsync(Department deprtment);
    }
}
