using HospitalManagement.Models;

namespace HospitalManagement.Services.Interface
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartmentAsync();
        Task<Department> UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(Department department);
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department?> AddDepartmentAsync(Department deprtment);
    }
}
