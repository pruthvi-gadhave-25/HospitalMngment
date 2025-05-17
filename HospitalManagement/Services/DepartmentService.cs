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
        public async  Task<Department?> AddDepartmentAsync(Department deprtment)
        {
            try
            {       if (deprtment == null)
                    return null;

                    return  await _departmentRepo.AddDepartmentAsync(deprtment);
               
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        public async  Task DeleteDepartmentAsync(Department department)
        {
            throw new NotImplementedException();
        }

        public  async Task<List<Department>> GetDepartmentAsync()
        {
            try
            {
                var res =  await _departmentRepo.GetAllDepartmentsAsync();
                return res ;
            } catch (Exception ex)
            {   
                //logger exception
                return new List<Department>() ;
            }
        }

        public async  Task<Department> GetDepartmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            throw new NotImplementedException();
        }
    }
}
