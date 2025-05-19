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

        public async  Task<Department?> GetDepartmentByIdAsync(int id)
        {
            try
            {
                return await _departmentRepo.GetDepartmentByIdAsync(id);
            }
            catch(Exception ex)
            {   
                return null;
            }
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            try
            {
                Department exisitngDept =await  _departmentRepo.GetDepartmentByIdAsync(department.Id);
                if(exisitngDept == null)
                {
                    return false;
                }
                return await _departmentRepo.UpdateDepartmentAsync(exisitngDept);
            }         
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var isIdExists = _departmentRepo.GetDepartmentByIdAsync(id);
                if (isIdExists == null)
                {
                    return false;
                }
                return await _departmentRepo.DeleteDepartmentAsync(id);
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
