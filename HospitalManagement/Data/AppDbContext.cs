using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients{get ;set;}
    }
}
