using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class DepartmentRepository :IDepartmentRepository
    {
        private readonly AppDbContext _context;


        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> AddDepartmentAsync(Department department)
        {

            try
            {
                if(department == null)
                {
                    return null;
                }
                var res = await _context.AddAsync(department);
                await _context.SaveChangesAsync();
                return res.Entity;
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Task DeleteDepartmentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async  Task<List<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                var deprtments = await _context.Departments.ToListAsync();

                return deprtments ?? new List<Department>();
            }
            catch (Exception ex)
            {
                //log edexception 

                return  new List<Department>();
            }
        }

        public Task<Department> GetDepartmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDepartmentAsync(Department department)
        {
            throw new NotImplementedException();
        }
    }
}
