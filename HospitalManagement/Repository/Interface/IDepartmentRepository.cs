using HospitalManagement.Models;

namespace HospitalManagement.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> AddDepartmentAsync(Department department);
        Task<bool> UpdateDepartmentAsync(Department department);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
