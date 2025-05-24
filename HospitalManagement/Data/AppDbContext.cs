using HospitalManagement.Models;
using HospitalManagement.Models.Helpers;
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
        public DbSet<Appointment>  Appointments { get; set; }


        public DbSet<Specialization>Specializations { get; set; }
        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }

        public DbSet<LeaveManagment>LeaveManagments { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Role>Roles { get; set; }
    }
}
