using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repository.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repository
{
    public class DepartmentRepository :GenericRepository<Department> ,IDepartmentRepository
    {
       
        public DepartmentRepository(AppDbContext context) : base(context)
        {
            
        }

               
               
    }
}
