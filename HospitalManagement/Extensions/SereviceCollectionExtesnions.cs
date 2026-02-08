using HospitalManagement.Data.UnitOfWork;
using HospitalManagement.Helpers;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Interface;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services;
using HospitalManagement.Services.Interface;

namespace HospitalManagement.Extensions
{
    public static class SereviceCollectionExtesnions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection  services)
        {
            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitofWork>();
            
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            //repository  
            services.AddScoped<PatientRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<DoctorRepository>();
            services.AddScoped<AppointmentRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<LeaveManagementRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            // Services
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IServiceLeaveManagement, LeaveService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(); //need manual injection  
            // no need to inject ilogger manyally  
            return services; 
        }

        public static IServiceCollection  AddInfrastructureServices (this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailSendService>();
            return services;
        }
    }
}
