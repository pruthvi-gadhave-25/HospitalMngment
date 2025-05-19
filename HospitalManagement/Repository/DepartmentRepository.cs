using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
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

    

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            try
            {
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"exception occurs {e.Message}");

                return false;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(p => p.Id == id);

                if (department == null)
                {
                    return false;
                }
                var res = _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var dept  = await _context.Departments.FindAsync(id);
                if(dept == null)
                {
                    return null; 
                }
                return dept;
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
